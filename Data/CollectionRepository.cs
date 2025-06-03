using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Interfaces;
using Microsoft.Data.SqlClient;
using Logic.Models;

namespace Data
{
    public class CollectionRepository : ICollectionRepository
    {
        private readonly string _connectionString;


        public CollectionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateCollection(Collection collection)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand(@"
                INSERT INTO Collection (title, uploadDate, owner)
                VALUES (@Title, @UploadDate, @Owner);
                SELECT SCOPE_IDENTITY();", conn);

            cmd.Parameters.AddWithValue("@Title", collection.Title);
            cmd.Parameters.AddWithValue("@UploadDate", collection.UploadDate);
            cmd.Parameters.AddWithValue("@Owner", collection.Owner);

            collection.Id = Convert.ToInt32(cmd.ExecuteScalar());
        }

        public List<Collection> GetCollectionsByOwner(string owner)
        {
            var collections = new List<Collection>();

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand("SELECT id, title, uploadDate FROM Collection WHERE owner = @Owner", conn);
            cmd.Parameters.AddWithValue("@Owner", owner);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                collections.Add(new Collection(
                    reader.GetInt32(0),                  // id
                    reader.GetString(1),                 // title
                    reader.GetDateTime(2),               // uploadDate
                    owner                                // gebruik parameterwaarde
                ));
            }

            return collections;
        }

        public Collection GetCollectionById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand("SELECT id, title, uploadDate, owner FROM Collection WHERE id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
                return null;

            var collection = new Collection(
                reader.GetInt32(0),       // id
                reader.GetString(1),      // title
                reader.GetDateTime(2),    // uploadDate
                reader.GetString(3)       // owner
            );

            reader.Close();

            // ✅ Laad ook de gekoppelde artworks
            var artworkCmd = new SqlCommand(@"
        SELECT a.id, a.title, a.description, a.owner, a.uploadDate, a.imageUrl
        FROM Artwork a
        INNER JOIN CollectionArtworks ca ON a.id = ca.ArtworkId
        WHERE ca.CollectionId = @CollectionId", conn);

            artworkCmd.Parameters.AddWithValue("@CollectionId", id);

            using var artworkReader = artworkCmd.ExecuteReader();
            while (artworkReader.Read())
            {
                collection.Artworks.Add(new Artwork(
                    artworkReader.GetInt32(0),
                    artworkReader.GetString(1),
                    artworkReader.GetString(2),
                    artworkReader.GetString(3),
                    artworkReader.GetDateTime(4),
                    artworkReader.GetString(5)
                ));
            }

            return collection;
        }


        public void AddArtworksToCollection(int collectionId, List<int> artworkIds)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            foreach (var id in artworkIds)
            {
                var cmd = new SqlCommand(
                    "INSERT INTO CollectionArtworks (CollectionId, ArtworkId) VALUES (@CollectionId, @ArtworkId)", conn);
                cmd.Parameters.AddWithValue("@CollectionId", collectionId);
                cmd.Parameters.AddWithValue("@ArtworkId", id);
                cmd.ExecuteNonQuery();
            }
        }
        public void AddArtworkToCollectionIfNotExists(int collectionId, int artworkId)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            // Controleer of de combinatie al bestaat
            var checkCmd = new SqlCommand(
                @"SELECT COUNT(*) FROM CollectionArtworks WHERE CollectionId = @CollectionId AND ArtworkId = @ArtworkId", conn);
            checkCmd.Parameters.AddWithValue("@CollectionId", collectionId);
            checkCmd.Parameters.AddWithValue("@ArtworkId", artworkId);

            int count = (int)checkCmd.ExecuteScalar();

            if (count == 0)
            {
                var insertCmd = new SqlCommand(
                    @"INSERT INTO CollectionArtworks (CollectionId, ArtworkId) VALUES (@CollectionId, @ArtworkId)", conn);
                insertCmd.Parameters.AddWithValue("@CollectionId", collectionId);
                insertCmd.Parameters.AddWithValue("@ArtworkId", artworkId);
                insertCmd.ExecuteNonQuery();
            }
        }
        public void DeleteCollection(int collectionId, string owner)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            // 1. Eerst verwijder je afhankelijkheden in koppel-tabel (foreign key constraint)
            var deleteLinksCmd = new SqlCommand(
                "DELETE FROM CollectionArtworks WHERE CollectionId = @Id", conn);
            deleteLinksCmd.Parameters.AddWithValue("@Id", collectionId);
            deleteLinksCmd.ExecuteNonQuery();

            // 2. Dan pas de collectie zelf, met eigenaarscheck (optioneel)
            var deleteCmd = new SqlCommand(
                "DELETE FROM Collection WHERE Id = @Id AND Owner = @Owner", conn);
            deleteCmd.Parameters.AddWithValue("@Id", collectionId);
            deleteCmd.Parameters.AddWithValue("@Owner", owner);
            deleteCmd.ExecuteNonQuery();
        }


    }
}
