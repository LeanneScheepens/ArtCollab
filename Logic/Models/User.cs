namespace ArtCollab.Models
{
    public class User
    {

        //private static int seed = 1;
        private int _Id;
        private string _Name;
        private string _Email;
        private string _Password;
        private string _ProfilePicture;
        private string _Biography;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }   
        public string Password { get; set; }
        public string ProfilePicture { get; set; }
        public string Biography { get; set; }

        public User (int id, string name, string email, string password, string profilePicture, string biography)
        {
            _Id = id;
            _Name = name;
            _Email = email;
            _Password = password;
            _ProfilePicture = profilePicture;
            _Biography = biography;
        }
    }
}
