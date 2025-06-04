using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Models;

namespace Logic.Models
{
    public class Admin : User
    {
        public Role Role { get; set; }

        public Admin(int id, string name, string email, string password, string profilePicture, string biography, int role) : base(id, name, email, password, profilePicture, biography)
        {
            Role = (Role)role;
        }
    }
}
