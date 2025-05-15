using ArtCollab.Models;
using Logic.Managers;
using Logic.Models;
using Logic.ViewModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace ArtCollab.Pages
{
    public class NewArtworkModel : PageModel
    {
        private readonly ArtworkManager _artworkManager;

        public NewArtworkModel(ArtworkManager artworkManager)
        {
            _artworkManager = artworkManager;
        }

        [BindProperty]
        public CreateArtworkViewModel ArtworkVM { get; set; }

        public void OnGet()
        { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            string uniqueFileName = null;
            if (ArtworkVM.ImageFile != null)
            {
                string uploadsFolder = Path.Combine("wwwroot", "images");
                Directory.CreateDirectory(uploadsFolder); // Zorg dat folder bestaat

                uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ArtworkVM.ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    ArtworkVM.ImageFile.CopyTo(fileStream);
                }
            }

            var newArtwork = new Artwork(
          id: 0, 
          title: ArtworkVM.Title,
          description: ArtworkVM.Description,
          owner: ArtworkVM.Owner,
          uploadDate: DateTime.Now,
          imageUrl: "/images/" + uniqueFileName
            );

            //// UserIds nog apart instellen:
            //newArtwork.UserIds = ArtworkVM.UserIds.Split(',').Select(id => int.Parse(id.Trim())).ToList();

            _artworkManager.CreateArtwork(newArtwork);

            return RedirectToPage("/ArtworkOverview"); 
        }

    }
}

