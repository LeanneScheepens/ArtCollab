using Logic.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Logic.Managers;
using Logic.Models;
using Logic.ViewModels;

namespace ArtCollab.Pages
{
    public class LoginModel : PageModel
    {
        private readonly UserManager _userManager;

        public LoginModel(UserManager userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        [Required]
        public string Name { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.AuthenticateUser(Name, Password);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            if (user.Role == Role.Admin)
                return RedirectToPage("/Admin/Dashboard");
            else
                return RedirectToPage("/Artist/Dashboard");
        }
    }
}
