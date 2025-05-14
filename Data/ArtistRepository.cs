using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtCollab.Data;
using ArtCollab.Models;
using Logic.Models;
using Logic.Interfaces;
using Microsoft.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace Data
{
    public class ArtistRepository : DatabaseService, IArtistRepository
    {

        public ArtistRepository(string ServicesString) : base(ServicesString) { } //to use the database service

        public List<Artist> GetArtists()
        {
            List<Artist> artists = new List<Artist>();

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                string sql = @"SELECT 
                        a.ArtistId,
                        u.Name,
                        u.Password,
                        u.Email,
                        u.ProfilePicture,
                        u.Biography
                      FROM Artist a
                      JOIN [User] u ON a.UserId = u.Id";
                //rol moet er nog bij

                using var command = new SqlCommand(sql, connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var artist = new Artist(
                        reader.GetInt32(0),   // ArtistId
                        reader.GetString(1),  // Name
                        reader.GetString(3),  // Email
                        reader.GetString(2),  // Password
                        reader.GetString(4),  // ProfilePicture
                        reader.GetString(5),  // Biography
                        0                     // Dummy Role (niet meer in DB aanwezig)
                    );
                    artists.Add(artist);
                }
            }

            return artists;
        }

        public Artist GetArtistById(int id)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                string sql = @"SELECT 
                        a.ArtistId, 
                        u.Name, 
                        u.Password, 
                        u.Email, 
                        u.ProfilePicture, 
                        u.Biography
                      FROM Artist a
                      JOIN [User] u ON a.UserId = u.Id
                      WHERE a.ArtistId = @Id";
                //rol moet er nog bij

                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Artist(
                        reader.GetInt32(0),   // ArtistId
                        reader.GetString(1),  // Name
                        reader.GetString(3),  // Email
                        reader.GetString(2),  // Password
                        reader.GetString(4),  // ProfilePicture
                        reader.GetString(5),  // Biography
                        0                     // Dummy Role
                    );
                }

                return null;
            }
        }

        public void DeleteArtist(int id)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string sql = "DELETE FROM Artist WHERE ArtistId = @Id";

                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

        public void CreateArtist(Artist artist)
        {
            Console.WriteLine("Repo:" + artist.Name);
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Stap 1: voeg gebruiker toe aan [User]
                    string insertUserSql = @"
                INSERT INTO [User] (Name, Email, Password, ProfilePicture, Biography)
                OUTPUT INSERTED.Id
                VALUES (@Name, @Email, @Password, @ProfilePicture, @Biography)";
                    //rol moet er nog bij

                    int userId;
                    using (var userCmd = new SqlCommand(insertUserSql, connection, transaction))
                    {
                        userCmd.Parameters.AddWithValue("@Name", artist.Name);
                        userCmd.Parameters.AddWithValue("@Email", artist.Email);
                        userCmd.Parameters.AddWithValue("@Password", artist.Password);
                        userCmd.Parameters.AddWithValue("@ProfilePicture", artist.ProfilePicture ?? (object)DBNull.Value);
                        userCmd.Parameters.AddWithValue("@Biography", artist.Biography ?? (object)DBNull.Value);

                        userId = (int)userCmd.ExecuteScalar();
                    }

                    // Stap 2: voeg artiest toe aan Artist + RoleId
                    string insertArtistSql = "INSERT INTO Artist (UserId) VALUES (@UserId)";
                    using (var artistCmd = new SqlCommand(insertArtistSql, connection, transaction))
                    {
                        artistCmd.Parameters.AddWithValue("@UserId", userId);
                        artistCmd.ExecuteNonQuery();
                    }

                    // Eventueel stap 3: voeg artist-role mapping toe aan een aparte tabel (indien je die hebt)

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public void UpdateArtist(Artist artist)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                string sql = @"
                         UPDATE u
                         SET u.Name = @Name,
                         u.Password = @Password
                         u.Email = @Email
                         u.ProfilePicture = @ProfilePicture
                         u.Biography = @Biography
                        FROM User u
                        JOIN Artist a ON a.UserId = p.Id
                        WHERE a.ArtistId = @ArtistId;

                       UPDATE Artist
                       WHERE ArtistId = @ArtistId;";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", artist.Name);
                    command.Parameters.AddWithValue("@Password", artist.Password);
                    command.Parameters.AddWithValue("@Email", artist.Email);
                    command.Parameters.AddWithValue("@ProfilePicture", artist.ProfilePicture ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Biography", artist.Biography ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CustomerId", artist.Id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }

}


    
