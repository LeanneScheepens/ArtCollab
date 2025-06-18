using Logic.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Logic.Managers;
using Logic.Models;
using Logic.ViewModels;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

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
        public RegisterViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                if (!IsValidPassword(ViewModel.Password))
                {
                    ModelState.AddModelError("ViewModel.Password", "Password must contain at least 4 digits and 3 letters.");
                    return Page();
                }

                var existingUser = _userManager.GetUserByName(ViewModel.Name);
                if (existingUser != null)
                {
                    ModelState.AddModelError("ViewModel.Name", "Username already exists.");
                    return Page();
                }

                var admin = new User(0, ViewModel.Name, ViewModel.Email, ViewModel.Password, null, null)
                {
                    Role = Role.Admin
                };

                _userManager.CreateUser(ViewModel, Role.Admin);  // Create the user in the database

                // Authenticate the user after registration
                var profilePicture = admin.ProfilePicture ?? "default.png";

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, admin.Name),
                    new Claim(ClaimTypes.Email, admin.Email ?? ""),
                    new Claim(ClaimTypes.Role, admin.Role.ToString()),
                    new Claim("ProfilePicture", profilePicture)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Sign the user in
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Redirect to the Admin Dashboard or other page after successful login
                return RedirectToPage("/UserOverview");
            }
            catch (ArgumentException ex)
            {
                // Handle any errors, for example, username already exists
                ModelState.AddModelError("ViewModel.Name", ex.Message);
                return Page();
            }
        }

        private bool IsValidPassword(string password)
        {
            int digits = Regex.Matches(password, @"\d").Count;
            int letters = Regex.Matches(password, @"[A-Za-z]").Count;
            return digits >= 4 && letters >= 3;
        }
    }
}
