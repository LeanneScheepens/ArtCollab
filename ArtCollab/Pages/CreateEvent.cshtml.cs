using ArtCollab.Models;
using ArtCollab.Services;
using Logic.Managers;
using Logic.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArtCollab.Pages
{
    [Authorize(Roles = "Admin")]
    public class CreateEventModel : PageModel
    {
        private readonly EventManager _eventManager;
        private readonly ArtworkManager _artworkManager;

        public CreateEventModel(EventManager eventManager, ArtworkManager artworkManager)
        {
            _eventManager = eventManager;
            _artworkManager = artworkManager;
        }

        [BindProperty]
        public CreateEventViewModel ViewModel { get; set; }

        public void OnGet()
        {
            ViewModel = new CreateEventViewModel
            {
                AvailableArtworks = _artworkManager.GetArtworks()
                    .Select(a => (a.Id, a.Title)).ToList()
            };
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ViewModel.AvailableArtworks = _artworkManager.GetArtworks()
                    .Select(a => (a.Id, a.Title)).ToList();
                return Page();
            }

            var owner = User.Identity?.Name ?? "Unknown";
            var evt = new Event(0, ViewModel.Title, ViewModel.Description, owner);

            _eventManager.CreateEvent(evt, ViewModel.SelectedArtworkIds);

            return RedirectToPage("/Events/Index");
        }

    }
}
