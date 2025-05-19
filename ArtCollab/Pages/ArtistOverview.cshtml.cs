//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Logic.Managers;
//using Logic.Interfaces;
//using Logic.Models;
//using Data;

//namespace ArtCollab.Pages
//{
//    public class ArtistOverviewModel(UserManager artistManager) : PageModel
//    {
//        [BindProperty]
//        public List<int> SelectedArtistIds { get; set; } = new();
//        private readonly UserManager _artistManager = artistManager;

//        public List<Artist> Artists { get; set; } = [];

//        public void OnGet()
//        {
//           Artists = _artistManager.GetArtists();
//        }
//        public IActionResult OnPostDeleteSelected()
//        {
//            if (SelectedArtistIds.Any())
//            {
//                _artistManager.DeleteArtist(SelectedArtistIds);
//            }

//            return RedirectToPage("/ArtistOverview");
//        }
//    }
//}

