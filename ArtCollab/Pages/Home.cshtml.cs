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
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        private const int PageSize = 12;
        public List<Artwork> Artworks { get; set; } = new();

        public void OnGet(int page = 1)
        {

            var allArtworks = _artworkManager.GetArtworks();
            CurrentPage = page;
            TotalPages = (int)Math.Ceiling(allArtworks.Count / (double)PageSize);

            Artworks = allArtworks
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

    }
}
