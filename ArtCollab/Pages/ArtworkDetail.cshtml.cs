using ArtCollab.Models;
using Logic.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArtCollab.Pages
{
    public class ArtworkDetailModel : PageModel
    {
        private readonly ArtworkManager _artworkManager;

        public ArtworkDetailModel(ArtworkManager artworkManager)
        {
            _artworkManager = artworkManager;
        }

        public Artwork Artwork { get; set; }

        public IActionResult OnGet(int id)
        {
            Artwork = _artworkManager.GetArtworkById(id);
            if (Artwork == null)
            {
                return RedirectToPage("/ArtworkOverview");
            }

            return Page();
        }
    }
}
