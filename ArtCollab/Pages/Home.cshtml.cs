using Logic.Models;
using Logic.Managers;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArtCollab.Pages
{
    public class HomeModel : PageModel
    {
        private readonly ArtworkManager _artworkManager;

        public HomeModel(ArtworkManager artworkManager)
        {
            _artworkManager = artworkManager;
        }

        public List<Artwork> Artworks { get; set; } = new();

        public void OnGet()
        {
            Artworks = _artworkManager.GetArtworks();
        }
    }
}
