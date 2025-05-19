//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Logic.Managers;
//using Logic.Models;
//using Logic.ViewModels;
//using Microsoft.AspNetCore.Cryptography.KeyDerivation;
//using Data;

//namespace ArtCollab.Pages
//{
//    public class NewArtistModel : PageModel
//    {
//        private readonly UserManager _artistManager;

//        public NewArtistModel(UserManager artistManager)
//        {
//            _artistManager = artistManager;
//        }

//        [BindProperty]
//        public CreateArtistViewModel ArtistVM { get; set; }

//        public void OnGet()
//        { }

//        public IActionResult OnPost()
//        {
//            if (!ModelState.IsValid)
//            {
//                return Page();
//            }

//            var hashedPassword = HashPassword(ArtistVM.Password);

//            var newArtist = new Artist(
//                0,  // ID wordt door de database gegenereerd
//                ArtistVM.Name,
//                ArtistVM.Email,
//                hashedPassword,
//                "default.jpg",  // Voeg een default profielafbeelding toe
//                "", // Voeg een standaard biografie toe
//                (int)Role.Artist // Set de rol (kan ook worden ingevuld via het formulier)
//            );

//            _artistManager.CreateArtist(newArtist);

//            // Redirect naar de juiste pagina na succesvolle creatie van de artiest
//            return RedirectToPage("ArtistOverview"); // Werk dit bij naar de juiste pagina
//        }

//        // Helper method to hash the password securely
//        private string HashPassword(string password)
//        {
//            var salt = new byte[16]; // Generate a new salt (e.g., using RandomNumberGenerator)
//            new Random().NextBytes(salt);

//            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
//                password: password,
//                salt: salt,
//                prf: KeyDerivationPrf.HMACSHA256,
//                iterationCount: 10000,
//                numBytesRequested: 256 / 8));

//            return hashed;
//        }
//    }
//}

