using Logic.Interfaces;

namespace ArtCollab.Models
{
    public class Event
    {

        private int id;
        private string title;
        private DateTime startDate;
        private DateTime endDate;
        private string description;
        private string owner;
        
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }

        public Event (int id, string title, DateTime startDate, DateTime endDate, string description, string owner)
        {
            Id = id;
            Title = title;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
            Owner = owner;
        }
        public List<Artwork> Artworks { get; set; } = new();


        public void AddArtworkAndPersist(Artwork artwork, IEventRepository repository)
        {
            if (!Artworks.Any(a => a.Id == artwork.Id))
            {
                Artworks.Add(artwork);
                repository.AddArtworksToEvent(this.Id, new List<int> { artwork.Id });
            }
        }


    }
}
