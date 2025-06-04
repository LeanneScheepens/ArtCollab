using System.Globalization;

namespace Logic.Models
{
    public class Comment
    {
        private int id;
        private string content;
        private string author;
        private DateTime uploadDate;


        public int Id { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime UploadDate{ get; set; }
        public int ArtworkId { get; set; }


        public Comment(int id, int artworkId, string content, string author, DateTime uploadDate)
        {
            Id = id;
            ArtworkId = artworkId;
            Content = content;
            Author = author;
            UploadDate = uploadDate;
        }
    }
}
