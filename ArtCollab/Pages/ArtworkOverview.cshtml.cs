
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
      

        private readonly ArtworkManager _artworkManager;

        public List<Artwork> Artworks { get; set; }

        public ArtworkOverviewModel(ArtworkManager artworkManager)
        {
            _artworkManager = artworkManager;
        }

        public void OnGet()
        {
            var currentUser = User.Identity?.Name;
            if (string.IsNullOrEmpty(currentUser))
            {
                Artworks = new List<Artwork>();
                return;
            }

            Artworks = _artworkManager.GetArtworks()
                .Where(a => a.Owner == currentUser)
                .ToList();
        }
    }
}