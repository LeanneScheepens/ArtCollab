using Logic.Models;
using Logic.Managers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure;
using Microsoft.AspNetCore.Mvc;

namespace ArtCollab.Pages
{
    public class HomeModel : PageModel
    {
        private readonly ArtworkManager _artworkManager;

        public HomeModel(ArtworkManager artworkManager)
        {
            _artworkManager = artworkManager;
        }
        [BindProperty(SupportsGet = true)]
        public int Page { get; set; } = 1;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        private const int PageSize = 12;
        public List<Artwork> Artworks { get; set; } = new();

        public IActionResult OnGet()
        {
            if (int.TryParse(Request.Query["page"], out int parsedPage))
            {
                Page = parsedPage;
            }

            CurrentPage = Page;

        var allArtworks = _artworkManager.GetArtworks();
            CurrentPage = Page;
            TotalPages = (int)Math.Ceiling(allArtworks.Count / (double)PageSize);

            Artworks = allArtworks
                .Skip((Page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            return Page();
        }


    }
}
