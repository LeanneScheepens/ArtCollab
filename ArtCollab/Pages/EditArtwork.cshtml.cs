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

            var existingArtwork = _artworkManager.GetArtworkById(ViewModel.ArtworkId);
            if (existingArtwork == null)
            {
                return NotFound();
            }

            existingArtwork.Title = ViewModel.Title;
            existingArtwork.ImageUrl = ViewModel.ImageUrl;
            existingArtwork.Owner = ViewModel.Owner;
            existingArtwork.Description = ViewModel.Description;
            existingArtwork.UploadDate = ViewModel.UploadDate;

            _artworkManager.UpdateArtwork(existingArtwork);

            return RedirectToPage("./ArtworkDetail", new { id = existingArtwork.Id });

        }
    }
}