
namespace ArtCollab.Models
{
    public class Artwork
    {

        private static int seed = 1;
        private int id;
        private string title;
        private string description;
        private string url;
        private string owner;

        public int Id { get; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }

        public Artwork (int id, string title, string description, string url, string owner)
        {
            this.id = seed;
            seed++;
            this.title = title;
            this.description = description;
            this.url = url;
            this.owner = owner;
        }

    }
}
