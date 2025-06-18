using Logic.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Logic.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace ArtCollab.Pages
{
    [Authorize]
    public class EditProfileModel : PageModel
    {
        private readonly UserManager _userManager;

        public EditProfileModel(UserManager userManager)
        {
            _userManager = userManager;
        }

        [BindProperty] public string Email { get; set; }
        [BindProperty] public string Biography { get; set; }
        [BindProperty] public IFormFile ProfileImage { get; set; }

        public void OnGet()
        {
            var user = _userManager.GetUserByName(User.Identity.Name);
            Email = user.Email;
            Biography = user.Biography;
        }

        public IActionResult OnPost()
        {
            var user = _userManager.GetUserByName(User.Identity.Name);
            if (user == null) return RedirectToPage("/Login");

            // Ensure we don't overwrite the email (since it's read-only and can't be modified)
            user.Email = user.Email;  // Ensuring that the email remains the same

            // Update the Biography and ProfilePicture
            user.Biography = Biography;

            if (ProfileImage != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);
                var path = Path.Combine("wwwroot", "Images", fileName);
                using var stream = new FileStream(path, FileMode.Create);
                ProfileImage.CopyTo(stream);
                user.ProfilePicture = fileName;
            }

            // Update the user in the database
            _userManager.UpdateUser(user);

            // Refresh session with new claims
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(ClaimTypes.Email, user.Email ?? ""),
        new Claim(ClaimTypes.Role, user.Role.ToString()),
        new Claim("ProfilePicture", user.ProfilePicture ?? "default.jpg")
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToPage("/Index");
        }

    }
}
