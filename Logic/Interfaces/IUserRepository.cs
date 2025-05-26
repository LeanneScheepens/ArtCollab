using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Models;

namespace Logic.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetUsers();
        User GetUserByName(string name);
        void CreateUser(User user);    
        void DeleteUser(int id);
        void UpdateUser(User user);

    }
}
