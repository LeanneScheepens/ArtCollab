using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ArtCollab.Pages.Data;

namespace ArtCollab.Pages
{
    public class TestConnection : PageModel
    {
        private readonly DatabaseServices _dbService;

        public string ConnectionStatus { get; set; }

        public TestConnection(IConfiguration config)
        {
            // Get the connection string from appsettings.json
            string connString = config.GetConnectionString("DefaultConnection");
            _dbService = new DatabaseServices(connString);
        }

        public void OnGet()
        {
            // Test the connection
            bool result = _dbService.TestConnection();
            ConnectionStatus = result ? "✅ Database connection successful!" : "❌ Database connection failed.";
        }
    }

}

