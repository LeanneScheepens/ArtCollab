namespace ArtCollab.Models
{
    public class Event
    {

        private static int seed = 1;
        private int id;
        private string title;
        private string description;
        private string owner;
        
        public int Id { get; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }

        public Event (int id, string title, string description, string owner)
        {
            Id = seed;
            seed++;
            this.title = title;
            this.description = description;
            this.owner = owner;
        }
    }
}
