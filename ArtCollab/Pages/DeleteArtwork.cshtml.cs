using Logic.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArtCollab.Pages
{
    public class DeleteArtworkModel : PageModel
    {
        private readonly ArtworkManager _artworkManager;

        public DeleteArtworkModel(ArtworkManager artworkManager)
        {
            _artworkManager = artworkManager;
        }

        public IActionResult OnGet(int id)
        {
            _artworkManager.DeleteArtwork(new List<int> { id });

            return RedirectToPage("/ArtworkOverview");
        }
    }
}
