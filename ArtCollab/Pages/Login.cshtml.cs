using Logic.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Logic.Managers;
using Logic.Models;
using Logic.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

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

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(ClaimTypes.Email, user.Email ?? ""),
        new Claim(ClaimTypes.Role, user.Role.ToString()),
        new Claim("ProfilePicture", user.ProfilePicture ?? "default.jpg")
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToPage("/ArtworkOverview");
        }

    }
}
