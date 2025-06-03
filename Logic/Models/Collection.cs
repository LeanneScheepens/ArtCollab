using Logic.Interfaces;
using Logic.Models;
using Logic.Managers;



namespace Logic.Models
{
    public class Collection
    {
        private int id;
        private string title;
        private string uploadDate;
        private string owner;

        public int Id { get; set; }
        public string Title { get; set; }
        public string Owner { get; set; }
        public DateTime UploadDate { get; set; }
        public List<Artwork> Artworks { get; set; } = new();

        public Collection() { }

        public Collection (int id, string title, DateTime uploadDate, string owner)
        {
            Id = id;
            Title = title;
            UploadDate = uploadDate;
            Owner = owner;
        }
     
    }
}
