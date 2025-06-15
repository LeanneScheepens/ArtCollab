using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Models;
using Logic.Interfaces;
using Logic.Utils;
using Logic.ViewModels;

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
        private bool IsBase64Valid(string base64)
        {
            try
            {
                var data = Convert.FromBase64String(base64);
                return data.Length >= 48; // salt + hash
            }
            catch
            {
                return false;
            }
        }
        public void CreateUser(RegisterViewModel registerViewModel, Role role)
        {
            var user = new User(0, registerViewModel.Name, registerViewModel.Email, registerViewModel.Password, null, null);

            if (!IsBase64Valid(user.Password))
            {
                user.Password = PasswordHelper.HashPassword(user.Password);
            }

            user.Role = role;

            _userRepository.CreateUser(user);
        }



        public User GetUserByName(string name)
        {
            return _userRepository.GetUserByName(name);
        }


        public Task<User> AuthenticateUser(string name, string password)
        {
            var user = _userRepository.GetUserByName(name);

            if (user != null && PasswordHelper.VerifyPassword(password, user.Password))
            {
                return Task.FromResult(user);
            }

            return Task.FromResult<User>(null);
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
        public void UpdateUser(User user)
        {
            _userRepository.UpdateUser(user);
        }

    }
}



