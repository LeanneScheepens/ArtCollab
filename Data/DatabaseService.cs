using ArtCollab.Interface;
using Microsoft.Data.SqlClient;


namespace ArtCollab.Data
{
    public class DatabaseService : IDatabaseService
    {
        private readonly string _ConnectionString;

        public DatabaseService(string connectionString)
        {
            _ConnectionString = connectionString;
        }

        public SqlConnection GetConnection() //protected
        {
            if (string.IsNullOrEmpty(_ConnectionString))
            {
                throw new InvalidOperationException("Connection string is not initialized.");
            }
            return new SqlConnection(_ConnectionString);
        }


        public bool TestConnection()
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();
                return true;
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
