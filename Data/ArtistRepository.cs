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
            List<Artist> artists = new List<Artist>();  // Correcte declaratie van de lijst

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                string sql = @"SELECT 
                            a.ArtistId AS ArtistId,
                            u.Name AS ArtistName,
                            u.Password AS ArtistPassword,
                            u.Email AS ArtistEmail,
                            u.ProfilePicture AS ArtistProfilePicture,
                            u.Biography AS ArtistBiography,
                            a.Role AS ArtistRole
                       FROM Artist a
                       JOIN User u ON a.UserId = u.Id";

                using var command = new SqlCommand(sql, connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var artist = new Artist(
                        reader.GetInt32(0),   // ArtistId
                        reader.GetString(1),  // Name
                        reader.GetString(2),  // Password
                        reader.GetString(3),  // Email
                        reader.GetString(4),  // ProfilePicture
                        reader.GetString(5),  // Biography
                        reader.GetInt32(6)    // Role
                    );
                    artists.Add(artist);  // Voeg de artiest toe aan de lijst
                }
            }

            return artists;  // Retourneer de lijst van artiesten
        }

        public Artist GetArtistById(int id)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                string sql = @"
                        SELECT a.ArtistId, u.Name, u.Password, u.Email, u.ProfilePicture, u.Biography, a.Role
                        FROM Artist a
                        JOIN User u ON a.UserId = u.Id
                        WHERE a.ArtistId = @Id";

                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Artist(
                              reader.GetInt32(0),   //ArtistId
                              reader.GetString(1),  //name
                              reader.GetString(2),  //password
                              reader.GetString(3),  //email
                              reader.GetString(4),  //profilePicture
                              reader.GetString(5),  //biography
                              reader.GetInt32(6)    //ArtistRole 
                    );
                }
                else
                {
                    return null;
                }
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
                    string insertArtistUserSql = @"INSERT INTO User (Name, Password, Email, ProfilePicture, Biography, Role) 
                                           OUTPUT INSERTED.Id 
                                           VALUES (@Name, @Password, @Email, @ProfilePicture, @Biography, @Role)";
                    int artistUserId;
                    using (var cmd = new SqlCommand(insertArtistUserSql, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Name", artist.Name);
                        cmd.Parameters.AddWithValue("@Password", artist.Password);
                        cmd.Parameters.AddWithValue("@Email", artist.Email);
                        cmd.Parameters.AddWithValue("@ProfilePicture", artist.ProfilePicture);
                        cmd.Parameters.AddWithValue("@Biography", artist.Biography);
                        cmd.Parameters.AddWithValue("@Role", (int)artist.Role); // Store 
                        artistUserId = (int)cmd.ExecuteScalar(); 
                    }

                    // Insert into the Artist table
                    string insertArtistSql = "INSERT INTO Artist (UserId) VALUES (@UserId)";
                    using (var cmd = new SqlCommand(insertArtistSql, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@UserId", artistUserId);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();  // Commit the transaction if everything was successful
                }
                catch
                {
                    transaction.Rollback();  // Rollback if any exception occurs
                    throw;
                }
            }
        }

    }
}

    
