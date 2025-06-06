using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Models;

namespace Logic.Interfaces
{
    public interface IEventRepository
    {
        void CreateEvent(Event evt);
        List<Event> GetAllEvents();
        Event GetEventById(int id);
        void AddArtworksToEvent(int eventId, List<int> artworkIds);
        void DeleteEvent(int id);
    }
}
