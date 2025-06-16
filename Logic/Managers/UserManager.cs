using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Models;
using Logic.Interfaces;
using Logic.Utils;
using Logic.ViewModels;
using System.ComponentModel.DataAnnotations;

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

        public void CreateUser(RegisterViewModel registerViewModel, Role role)
        {
     
            var validationContext = new ValidationContext(registerViewModel);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(registerViewModel, validationContext, validationResults, true);

            if (!isValid)
            {
                string errorMessages = string.Join("; ", validationResults.Select(r => r.ErrorMessage));
                throw new ArgumentException($"Validation error: {errorMessages}");
            }

            var user = new User(0, registerViewModel.Name, registerViewModel.Email, registerViewModel.Password, null, null);

            user.Password = PasswordHelper.HashPassword(user.Password);
           
            user.Role = role;

            var existingUser = _userRepository.GetUserByName(registerViewModel.Name);
            if (existingUser != null)
            {
                throw new ArgumentException("Username already exists.");
            }

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

        public void UpdateUser(User user)
        {
            _userRepository.UpdateUser(user);
        }

    }
}



