using Logic.Managers;
using Logic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArtCollab.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class UserOverviewModel : PageModel
    {
        private readonly UserManager _userManager;

        public UserOverviewModel(UserManager userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public string SearchTerm { get; set; }

        [BindProperty]
        public string SelectedRole { get; set; }

        public List<User> Users { get; set; }

        [BindProperty]
        public List<int> SelectedUserIds { get; set; } = new();

        public void OnGet()
        {
            Users = _userManager.GetUsers();
        }

        public void OnPostSearch()
        {
            var allUsers = _userManager.GetUsers();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                allUsers = allUsers
                    .Where(u => u.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(SelectedRole))
            {
                if (Enum.TryParse<Role>(SelectedRole, out var roleEnum))
                {
                    allUsers = allUsers.Where(u => u.Role == roleEnum).ToList();
                }
            }

            Users = allUsers;
        }

        public IActionResult OnPostDelete()
        {
            if (SelectedUserIds.Any())
            {
                _userManager.DeleteUsers(SelectedUserIds);
            }
            return RedirectToPage();
        }
    }
}
