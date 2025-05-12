namespace ArtCollab.Models
{
    public class User
    {

        private static int seed = 1;
        private int id;
        private string name;
        private string email;
        private string password;
        private string profilePicture;
        private string biography;

        public int Id { get; }
        public string Name { get; set; }
        public string Email { get; set; }   
        public string Password { get; set; }
        public string ProfilePicture { get; set; }
        public string Biography { get; set; }

        public User (int id, string name, string email, string password, string profilePicture, string biography)
        {
            this.id = seed;
            seed++;
            this.name = name;
            this.email = email;
            this.password = password;
            this.profilePicture = profilePicture;
            this.biography = biography;
        }
    }
}
