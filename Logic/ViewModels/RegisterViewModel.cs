using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Logic.Managers;

namespace Logic.ViewModels
{
    public class RegisterViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Password != ConfirmPassword)
            {
                yield return new ValidationResult("Passwords do not match.", new[] { "ConfirmPassword" });
            }

            if (!IsValidPassword(Password))
            {
                yield return new ValidationResult("Password must contain at least 4 digits and 3 letters.", new[] { "Password" });
            }

            var userManager = (UserManager)validationContext.GetService(typeof(UserManager));
            var existingUser = userManager.GetUserByName(Name);
            if (existingUser != null)
            {
                yield return new ValidationResult("Username already exists.", new[] { "Name" });
            }
        }

        private bool IsValidPassword(string password)
        {
            var digits = Regex.Matches(password, @"\d").Count;
            var letters = Regex.Matches(password, @"[A-Za-z]").Count;
            return digits >= 4 && letters >= 3;
        }
    }
}
