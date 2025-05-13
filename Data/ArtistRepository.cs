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

namespace Data
{
    public class ArtistRepository : DatabaseService, IArtistRepository
    {
        public ArtistRepository(string ServicesString) : base(ServicesString) { }

        public void CreateArtist(Artist artist)
        {
            using var conn = GetConnection();
            conn.Open();

            var transaction = conn.BeginTransaction();

            try
            {
                var cmd = new SqlCommand("""
                                         INSERT INTO user (name, email, profilePicture, biography) 
                                         VALUES (@name, @email, @profilePicture, @biography)
                                         SELECT SCOPE_IDENTITY()
                                     """, conn) { Transaction = transaction };
                cmd.Parameters.AddWithValue("@name", artist.Name);
                cmd.Parameters.AddWithValue("@email", artist.Email);
                cmd.Parameters.AddWithValue("@profilePicture", artist.ProfilePicture);
                cmd.Parameters.AddWithValue("@biography", artist.Biography);
                var result = cmd.ExecuteScalar();

                var userId = Convert.ToInt64(result);

                cmd = new SqlCommand("""
                                     INSERT INTO [Artist] (role, userId)
                                     VALUES (@role, @userId)
                                 """, conn) { Transaction = transaction };
                cmd.Parameters.AddWithValue("@role", (int)artist.Role);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.ExecuteNonQuery();

                transaction.Commit();
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }

        public List<User> GetArtists()
        {
            throw new NotImplementedException();
        }


        public void DeleteArtist(int id)
        {
            throw new NotImplementedException();
        }

    }
}