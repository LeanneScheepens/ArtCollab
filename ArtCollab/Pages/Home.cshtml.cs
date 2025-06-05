using Logic.Models;
using Logic.Managers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure;
using Microsoft.AspNetCore.Mvc;

namespace ArtCollab.Pages
{
    public class HomeModel : PageModel
    {
        private const int PageSize = 12;
        private readonly ArtworkManager _artworkManager;

        public HomeModel(ArtworkManager artworkManager)
        {
            _artworkManager = artworkManager;
        }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int TotalPages { get; set; }
        public List<Artwork> Artworks { get; set; } = new();

        public void OnGet()
        {
            var allArtworks = _artworkManager.GetArtworks();
            TotalPages = (int)Math.Ceiling(allArtworks.Count / (double)PageSize);

            Artworks = allArtworks
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }
        public IActionResult OnPostDelete(int id)
        {
            if (!User.IsInRole("Admin"))
                return Forbid();

            _artworkManager.DeleteArtwork(new List<int> { id });
            return RedirectToPage(new { PageNumber });
        }

    }
}