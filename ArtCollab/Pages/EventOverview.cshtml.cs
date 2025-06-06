using Logic.Models;
using ArtCollab.Services;
using Logic.Interfaces;
using Logic.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArtCollab.Pages
{
    public class EventOverviewModel : PageModel
    {
        private readonly EventManager _eventManager;
        private readonly IEventRepository _eventRepository;
        private readonly ArtworkManager _artworkManager;

        public List<Artwork> Artworks { get; set; } = new();
        public List<Event> Events { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int? SelectedEventId { get; set; }

        public Event SelectedEvent => Events?.FirstOrDefault(e => e.Id == SelectedEventId);

        public EventOverviewModel(
            EventManager eventManager,
            IEventRepository eventRepository,
            ArtworkManager artworkManager)
        {
            _eventManager = eventManager;
            _eventRepository = eventRepository;
            _artworkManager = artworkManager;
        }

        public void OnGet()
        {
            Events = _eventManager.GetAllEvents();

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

        public IActionResult OnPostAddArtwork(int EventId, int ArtworkId)
        {
            var evt = _eventManager.GetEventById(EventId);
            if (evt == null) return NotFound();

            // In praktijk haal je de artwork op uit de database
            var artwork = new Artwork(ArtworkId, "placeholder", "desc", "owner", DateTime.Now, "url");

            evt.AddArtworkAndPersist(artwork, _eventRepository);

            return RedirectToPage(new { SelectedEventId = EventId });
        }
        public IActionResult OnPostDeleteEvent(int id)
        {
            if (!User.IsInRole("Admin"))
                return Forbid();

            _eventManager.DeleteEvent(id);

            return RedirectToPage(); // herlaadt de pagina zonder geselecteerd event
        }

    }
}
