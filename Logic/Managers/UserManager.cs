using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Models;
using Logic.Interfaces;
using Logic.Utils;

namespace Logic.Managers
{
    public class UserManager
    {
            private readonly IUserRepository _userRepository;

            public UserManager(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public List<User> GetUsers()
            {
                return _userRepository.GetUsers();
            }

            public void DeleteUsers(List<int> ids)
            {
                foreach (int id in ids)
                {
                    _userRepository.DeleteUser(id);
                }
            }

            public void CreateUser(User user)
            {
            user.Password = PasswordHelper.HashPassword(user.Password);
            _userRepository.CreateUser(user);
            }

        public User GetUserByName(string name)
        {
            return _userRepository.GetUserByName(name);
        }


        public Task<User> AuthenticateUser(string name, string password)
        {
            var users = _userRepository.GetUsers();

            var user = users.FirstOrDefault(u =>
                u.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);

            return Task.FromResult(user);
        }
        public void RegisterUser(User user)
        {
            user.Password = PasswordHelper.HashPassword(user.Password);
            _userRepository.CreateUser(user);
        }

        public bool ValidateLogin(string name, string password)
        {
            var user = _userRepository.GetUserByName(name);
            return user != null && PasswordHelper.VerifyPassword(password, user.Password);
        }
    }
    }



