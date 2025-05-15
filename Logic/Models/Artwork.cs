
namespace ArtCollab.Models
{
    public class Artwork
    {

        //private static int seed = 1;
        private int id;
        private string title;
        private string description;
        private string owner;
        private DateTime uploadDate;
        private string imageUrl;

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public DateTime UploadDate { get; set; }
        public string ImageUrl { get; set; }

        //public List<int> UserIds { get; set; } = new();

        public Artwork (int id, string title, string description, string owner, DateTime uploadDate, string imageUrl)
        {
            Id = id;
            Title = title;
            Description = description;
            Owner = owner;
            UploadDate = uploadDate;
            ImageUrl = imageUrl;
        }

    }
}
