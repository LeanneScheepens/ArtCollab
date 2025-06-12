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
        public LoginViewModel Login { get; set; } // Gebruik de ViewModel

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // Authenticeer de gebruiker
            var user = await _userManager.AuthenticateUser(Login.Name, Login.Password);

            // Als de gebruiker niet gevonden is, voeg dan een foutmelding toe aan ModelState
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return Page(); // Dit zorgt ervoor dat de foutmelding wordt weergegeven op de pagina
            }

            // Als authenticatie succesvol is, ga dan verder met inloggen
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(ClaimTypes.Email, user.Email ?? ""),
        new Claim(ClaimTypes.Role, user.Role.ToString()),
        new Claim("ProfilePicture", user.ProfilePicture ?? "default.jpg")
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            bool keepLoggedIn = Login.KeepLoggedIn;

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = keepLoggedIn,
                ExpiresUtc = keepLoggedIn ? DateTime.UtcNow.AddDays(14) : (DateTime?)null
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

            return RedirectToPage("/ArtworkOverview");
        }

    }
}