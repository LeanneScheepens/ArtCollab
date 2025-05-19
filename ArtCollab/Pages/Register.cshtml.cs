using Logic.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Logic.Managers;
using Logic.Models;
using Logic.ViewModels;

namespace ArtCollab.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager _userManager;

        public RegisterModel(UserManager userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        [Required]
        public string Name { get; set; }

        [BindProperty]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [BindProperty] public string ConfirmPassword { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();
            if (Password != ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                return Page();
            }


            if (!IsValidPassword(Password))
            {
                ModelState.AddModelError("Password", "Password must contain at least 4 digits and 3 letters.");
                return Page();
            }

            var existingUser = _userManager.GetUserByName(Name); 
            if (existingUser != null)
            {
                ModelState.AddModelError("Name", "Username already exists.");
                return Page();
            }

            var user = new User(0, Name, Email, Password, null, null)
            {
                Role = Role.Artist
            };

            _userManager.CreateUser(user); 


            return RedirectToPage("/Home");
        }

        private bool IsValidPassword(string password)
        {
            var digits = Regex.Matches(password, @"\d").Count;
            var letters = Regex.Matches(password, @"[A-Za-z]").Count;
            return digits >= 4 && letters >= 3;
        }
    }
}
