using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtCollab.Data;
using ArtCollab.Models;
using Logic.Interfaces;
using Logic.Models;
using Microsoft.Data.SqlClient;

namespace Data
{
    public class ArtworkRepository : DatabaseService, IArtworkRepository
    {
        public ArtworkRepository(string ServicesString) : base(ServicesString) { } //to use the database service

        public List<Artwork> GetArtworks()
        {
            List<Artwork> artworks = new List<Artwork>();

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                string sql = @"SELECT 
                        id,
                        title,
                        description,
                        owner,
                        uploadDate,
                        imageUrl
                      FROM Artwork";
                //rol moet er nog bij

                using var command = new SqlCommand(sql, connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var artwork = new Artwork(
                        reader.GetInt32(0),   // ArtworkId
                        reader.GetString(1),  // title
                        reader.GetString(2),  // description
                        reader.GetString(3),  // owner
                        reader.GetDateTime(4),  // uploadDate
                        reader.GetString(5)   // imageUrl

                    );
                    artworks.Add(artwork);
                }
            }

            return artworks;
        }
        public void CreateArtwork(Artwork artwork)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    string insertArtworkSql = @"
                INSERT INTO Artwork (title, description, owner, uploadDate, imageUrl)
                OUTPUT INSERTED.id
                VALUES (@title, @description, @owner, @uploadDate, @imageUrl)";

                    int ArtworkId;
                    using (var artworkCmd = new SqlCommand(insertArtworkSql, connection, transaction))
                    {
                        artworkCmd.Parameters.AddWithValue("@Title", artwork.Title);
                        artworkCmd.Parameters.AddWithValue("@Description", artwork.Description ?? (object)DBNull.Value);
                        artworkCmd.Parameters.AddWithValue("@Owner", artwork.Owner);
                        artworkCmd.Parameters.AddWithValue("@UploadDate", artwork.UploadDate);
                        artworkCmd.Parameters.AddWithValue("@ImageUrl", artwork.ImageUrl ?? (object)DBNull.Value);

                        ArtworkId = (int)artworkCmd.ExecuteScalar();
                    }

                    //foreach (var userId in artwork.UserIds)
                    //{
                    //    string insertJoinSql = @"INSERT INTO Artwork_User (artworkId, userId)
                    //                     VALUES (@artworkId, @userId)";

                    //    using var joinCmd = new SqlCommand(insertJoinSql, connection, transaction);
                    //    joinCmd.Parameters.AddWithValue("@artworkId", ArtworkId);
                    //    joinCmd.Parameters.AddWithValue("@userId", userId);
                    //    joinCmd.ExecuteNonQuery();
                    //}

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public void DeleteArtwork(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                // Eerst relaties verwijderen
                var deleteRelationsCommand = connection.CreateCommand();
                deleteRelationsCommand.CommandText = "DELETE FROM Artwork_User WHERE artworkId = @Id";
                deleteRelationsCommand.Parameters.AddWithValue("@Id", id);
                deleteRelationsCommand.ExecuteNonQuery();

                // Daarna artwork zelf verwijderen
                var deleteArtworkCommand = connection.CreateCommand();
                deleteArtworkCommand.CommandText = "DELETE FROM Artwork WHERE id = @Id";
                deleteArtworkCommand.Parameters.AddWithValue("@Id", id);
                deleteArtworkCommand.ExecuteNonQuery();
            }
        }

        public Artwork GetArtworkById(int id)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                string sql = @"SELECT 
                        id,
                        title,
                        description,
                        owner,
                        uploadDate,
                        imageUrl
                      FROM Artwork
                      WHERE id = @Id";

                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Artwork(
                           reader.GetInt32(0),   // ArtworkId
                        reader.GetString(1),  // title
                        reader.GetString(2),  // description
                        reader.GetString(3),  // owner
                        reader.GetDateTime(4),  // uploadDate
                        reader.GetString(5)   // imageUrl
                    );
                }

                return null;
            }
        }
    }
}
