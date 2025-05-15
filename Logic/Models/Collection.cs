namespace ArtCollab.Models
{
    public class Collection
    {

        private static int seed = 1;
        private int id;
        private string name;
        private string uploadDate;

        public int Id { get; }
        public string Name { get; set; }
        public string UploadDate { get; set; }

        public Collection (int id, string name, string uploadDate)
        {
            Id = id;
            this.name = name;
            this.uploadDate = uploadDate;
        }
 
    }
}
