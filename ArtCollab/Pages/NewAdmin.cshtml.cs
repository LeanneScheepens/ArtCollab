using Logic.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Logic.Managers;
using Logic.Models;
using Logic.ViewModels;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;


namespace ArtCollab.Pages
{
    [Authorize(Roles = "Admin")]
    public class NewAdminModel : PageModel
    {
        private readonly UserManager _userManager;

        public NewAdminModel(UserManager userManager)
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

        [BindProperty]
        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public RegisterViewModel Register { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            if (!IsValidPassword(Password))
            {
                ModelState.AddModelError("Password", "Password must contain at least 4 digits and 3 letters.");
                return Page();
            }

            var existing = _userManager.GetUserByName(Name);
            if (existing != null)
            {
                ModelState.AddModelError("Name", "Username already exists.");
                return Page();
            }

            var admin = new User(0, Name, Email, Password, null, null)
            {
                Role = Role.Admin
            };

            _userManager.CreateUser(Register, Role.Admin);

            return RedirectToPage("/Privacy");
        }

        private bool IsValidPassword(string password)
        {
            int digits = Regex.Matches(password, @"\d").Count;
            int letters = Regex.Matches(password, @"[A-Za-z]").Count;
            return digits >= 4 && letters >= 3;
        }
    }
}

