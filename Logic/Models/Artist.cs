using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtCollab.Models;

namespace Logic.Models
{
    public class Artist : User
    {
        public Role Role { get; set; }

        public Artist(int id, string name, string email, string password, string profilePicture, string biography, int role) : base(id, name, email, password, profilePicture, biography)
        {
            Role = (Role)role;
        }
    }
}
