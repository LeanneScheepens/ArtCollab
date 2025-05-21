using Microsoft.Data.SqlClient;

namespace ArtCollab.Interface
{
    public interface IDatabaseService // kan weg
    {
        protected SqlConnection GetConnection();
    }
}
