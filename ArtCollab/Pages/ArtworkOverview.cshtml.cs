
using Logic.Models;
using Logic.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace ArtCollab.Pages
{
    [Authorize]
    public class ArtworkOverviewModel : PageModel
    {


        private const int PageSize = 12;
        private readonly ArtworkManager _artworkManager;

        public List<Artwork> Artworks { get; set; }

        public ArtworkOverviewModel(ArtworkManager artworkManager)
        {
            _artworkManager = artworkManager;
        }
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int TotalPages { get; set; }
        public void OnGet()
        {
            {
                var currentUser = User.Identity?.Name;
                if (string.IsNullOrEmpty(currentUser))
                {
                    Artworks = new List<Artwork>();
                    TotalPages = 0;
                    return;
                }

                var userArtworks = _artworkManager.GetArtworks()
                    .Where(a => a.Owner == currentUser)
                    .ToList();

                TotalPages = (int)Math.Ceiling(userArtworks.Count / (double)PageSize);

                Artworks = userArtworks
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();
            }
        }
    }
}