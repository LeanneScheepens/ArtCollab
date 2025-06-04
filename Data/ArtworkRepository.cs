using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ArtCollab.Data;
using Logic.Models;
using Logic.Interfaces;
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
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Verwijder gekoppelde EventArtworks records
                    var deleteEventArtworksCommand = connection.CreateCommand();
                    deleteEventArtworksCommand.Transaction = transaction;
                    deleteEventArtworksCommand.CommandText = "DELETE FROM EventArtworks WHERE artworkId = @Id";
                    deleteEventArtworksCommand.Parameters.AddWithValue("@Id", id);
                    deleteEventArtworksCommand.ExecuteNonQuery();

                    // Verwijder gekoppelde Artwork_User records
                    var deleteArtworkUserCommand = connection.CreateCommand();
                    deleteArtworkUserCommand.Transaction = transaction;
                    deleteArtworkUserCommand.CommandText = "DELETE FROM Artwork_User WHERE artworkId = @Id";
                    deleteArtworkUserCommand.Parameters.AddWithValue("@Id", id);
                    deleteArtworkUserCommand.ExecuteNonQuery();

                    // Verwijder het Artwork zelf
                    var deleteArtworkCommand = connection.CreateCommand();
                    deleteArtworkCommand.Transaction = transaction;
                    deleteArtworkCommand.CommandText = "DELETE FROM Artwork WHERE id = @Id";
                    deleteArtworkCommand.Parameters.AddWithValue("@Id", id);
                    deleteArtworkCommand.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
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
