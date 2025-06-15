namespace Logic.Models
{
    public class User
    {

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
        public Role Role { get; set; }

        public User ()
        {
    
        }
        public User(int id, string name, string email, string password, string profilePicture, string biography)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
            ProfilePicture = profilePicture;
            Biography = biography;
        }

    }
}
