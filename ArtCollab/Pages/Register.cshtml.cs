using Logic.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Logic.Managers;
using Logic.Models;
using Logic.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

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
            {
                return Page();
            }

            try
            {
                _userManager.CreateUser(Register, Role.Artist);

                var user = _userManager.GetUserByName(Register.Name);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Er is iets mis gegaan bij het ophalen van de gebruiker.");
                    return Page();
                }

                // Authenticate the user after registration
                var profilePicture = user.ProfilePicture ?? "default.png"; 

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email ?? ""),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim("ProfilePicture", profilePicture)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = false,  // Gebruiker niet persistent ingelogd houden
                    ExpiresUtc = DateTime.UtcNow.AddDays(14)  // Optionele vervaltijd voor de sessie
                };

                // Log de gebruiker in
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

                // Redirect naar de homepagina na succesvolle login
                return RedirectToPage("/Home");
            }
            catch (ArgumentException ex)
            {
                // Als er een fout optreedt, bijvoorbeeld de gebruikersnaam bestaat al
                ModelState.AddModelError("Register.Name", ex.Message);
                return Page();
            }
        }
    }
}