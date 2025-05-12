using Microsoft.Data.SqlClient;

namespace ArtCollab.Interface
{
    public interface IDatabaseService
    {
        protected SqlConnection GetConnection();
        public bool TestConnection();
    }
}
