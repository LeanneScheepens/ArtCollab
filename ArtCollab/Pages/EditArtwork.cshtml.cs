using Logic.Managers;
using Logic.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Logic.Interfaces;
using Data;


namespace ArtCollab.Pages
{
    [Authorize]
    public class EditArtworkModel : PageModel
    {
        private readonly ArtworkManager _artworkManager;

        public EditArtworkModel(ArtworkManager artworkManager)
        {
            _artworkManager = artworkManager;
        }

        [BindProperty]
        public EditArtworkViewModel ViewModel { get; set; }

        public IActionResult OnGet(int id)
        {
            var artwork = _artworkManager.GetArtworkById(id);
            if (artwork == null)
            {  
                return NotFound();
            }

            ViewModel = new EditArtworkViewModel
            {
                ArtworkId = artwork.Id,
                Title = artwork.Title,
                ImageUrl = artwork.ImageUrl,
                Owner = artwork.Owner,
                Description = artwork.Description,
                UploadDate = artwork.UploadDate
            };

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                _artworkManager.UpdateArtwork(ViewModel);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }

            return RedirectToPage("./ArtworkDetail", new { id = ViewModel.ArtworkId });

        }
    }
}