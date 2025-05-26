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
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
               
            };

        }

        public IActionResult OnPost()
        {
            var owner = User.Identity?.Name ?? "Unknown";

            var evt = new Event(
                0,
                ViewModel.Title,
                ViewModel.StartDate,
                ViewModel.EndDate,
                ViewModel.Description,
                owner
            );

            _eventManager.CreateEvent(evt);


            return RedirectToPage("/Events");
        }

    }
}
