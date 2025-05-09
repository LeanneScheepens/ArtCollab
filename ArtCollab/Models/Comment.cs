using System.Globalization;

namespace ArtCollab.Models
{
    public class Comment
    {
        private static int seed = 1;
        private int id;
        private string content;
        private string author;
        private int timeStamp;


        public int Id { get; }
        public string Content { get; set; }
        public string Author { get; set; }
        public int TimeStamp { get; set; }
        

        public Comment (int id, string content, string author, int timeStamp)
        {
            this.id = seed;
            seed++;
            this.content = content; 
            this.author = author;
            this.timeStamp = timeStamp;
        }    
    }
}
