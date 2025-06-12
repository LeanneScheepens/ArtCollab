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
        public RegisterViewModel Register { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = new User(0, Register.Name, Register.Email, Register.Password, null, null)
            {
                Role = Role.Artist
            };

            _userManager.CreateUser(user);

            return RedirectToPage("/Home");
        }
    }
}