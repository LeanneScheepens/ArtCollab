using ArtCollab.Models;
using Logic.Interfaces;

namespace ArtCollab.Services
{
    public class EventManager
    {
        private readonly IEventRepository _eventRepository;

        public EventManager(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public void CreateEvent(Event evt, List<int> artworkIds)
        {
            _eventRepository.CreateEvent(evt);
            _eventRepository.AddArtworksToEvent(evt.Id, artworkIds);
        }

        public List<Event> GetAllEvents() => _eventRepository.GetAllEvents();

        public Event GetEventById(int id) => _eventRepository.GetEventById(id);
    }
}
