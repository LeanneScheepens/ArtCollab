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
    public class UserRepository : DatabaseService, IUserRepository
    {
        public UserRepository(string ServicesString) : base(ServicesString) { }

        public List<User> GetUsers()
        {
            var users = new List<User>();

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                string sql = @"SELECT 
                        u.Id,
                        u.Name,
                        u.Email,
                        u.Password,
                        u.ProfilePicture,
                        u.Biography
                      FROM [User] u";

                using var command = new SqlCommand(sql, connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var user = new User(
                        reader.GetInt32(0),   // Id
                        reader.GetString(1),  // Name
                        reader.GetString(2),  // Email
                        reader.GetString(3),  // Password
                        reader.IsDBNull(4) ? null : reader.GetString(4),  // ProfilePicture
                        reader.IsDBNull(5) ? null : reader.GetString(5)   // Biography
                    );
                    users.Add(user);
                }
            }

            return users;
        }

        public User GetUserByName(string name)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                string sql = @"
            SELECT Id, Name, Email, Password, ProfilePicture, Biography, 'Artist' AS Role
            FROM [User]
            WHERE LOWER(Name) = LOWER(@Name)"
;

                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Name", name);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    var user = new User(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.IsDBNull(4) ? null : reader.GetString(4),
                        reader.IsDBNull(5) ? null : reader.GetString(5)
    )
                    {
                        Role = Role.Artist
                    };
                    return user;

                }
                return null;
            }
        }


        public void DeleteUser(int id)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string sql = "DELETE FROM [User] WHERE Id = @Id";

                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

        public void CreateUser(User user)
        {
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

                    int userId;
                    using (var userCmd = new SqlCommand(insertUserSql, connection, transaction))
                    {
                        userCmd.Parameters.AddWithValue("@Name", user.Name);
                        userCmd.Parameters.AddWithValue("@Email", user.Email);
                        userCmd.Parameters.AddWithValue("@Password", user.Password);
                        userCmd.Parameters.AddWithValue("@ProfilePicture", (object?)user.ProfilePicture ?? DBNull.Value);
                        userCmd.Parameters.AddWithValue("@Biography", (object?)user.Biography ?? DBNull.Value);

                        userId = (int)userCmd.ExecuteScalar();
                    }

                    // Stap 2: als de rol Artist is, voeg toe aan Artist-tabel
                    if (user.Role == Role.Artist)
                    {
                        string insertArtistSql = "INSERT INTO Artist (UserId) VALUES (@UserId)";
                        using (var artistCmd = new SqlCommand(insertArtistSql, connection, transaction))
                        {
                            artistCmd.Parameters.AddWithValue("@UserId", userId);
                            artistCmd.ExecuteNonQuery();
                        }
                    }
                    else if (user.Role == Role.Admin)
                    {
                        string insertAdminSql = "INSERT INTO Admin (UserId) VALUES (@UserId)";
                        using (var adminCmd = new SqlCommand(insertAdminSql, connection, transaction))
                        {
                            adminCmd.Parameters.AddWithValue("@UserId", userId);
                            adminCmd.ExecuteNonQuery();
                        }
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
        public void UpdateUser(User user)
        {
            using var connection = GetConnection();
            connection.Open();

            string sql = @"
        UPDATE [User]
        SET Email = @Email,
            Biography = @Biography,
            ProfilePicture = @ProfilePicture
        WHERE Name = @Name";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Biography", user.Biography ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@ProfilePicture", user.ProfilePicture ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Name", user.Name);

            command.ExecuteNonQuery();
        }


    }
}
