using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;


namespace ArtCollab.Pages.Data
{
    public class DatabaseServices
    {
        private readonly string _ServicesString;

        public DatabaseServices(string ServicesString)
        {
            // Haal de connection string op uit appsettings.json.
            _ServicesString = ServicesString;
        }

        protected SqlConnection GetConnection()
        {
            if (string.IsNullOrEmpty(_ServicesString))
            {
                throw new InvalidOperationException("Connection string is not initialized.");
            }
            return new SqlConnection(_ServicesString);
        }



        public bool TestConnection()
        {
            try
            {
                using (var connection = new SqlConnection(_ServicesString))
                {
                    connection.Open();
                    return connection.State == System.Data.ConnectionState.Open;
                }
            }
            catch (Exception ex)
            {
                // Eventueel loggen of tonen voor debugging
                Console.WriteLine("Connection failed: " + ex.Message);
                return false;
            }
        }
    }
}

