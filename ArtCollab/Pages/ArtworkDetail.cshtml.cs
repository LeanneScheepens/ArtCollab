using Logic.Models;
using Logic.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArtCollab.Pages
{
    public class ArtworkDetailModel : PageModel
    {
        private readonly ArtworkManager _artworkManager;
        private readonly CollectionManager _collectionManager;

        public ArtworkDetailModel(ArtworkManager artworkManager, CollectionManager collectionManager)
        {
            _artworkManager = artworkManager;
            _collectionManager = collectionManager;
        }

        [TempData]
        public string? SuccessMessage { get; set; }

        public Artwork Artwork { get; set; }

        [BindProperty]
        public int SelectedCollectionId { get; set; }

        public List<Logic.Models.Collection> UserCollections { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            Artwork = _artworkManager.GetArtworkById(id);
            if (Artwork == null) return RedirectToPage("/ArtworkOverview");

            var owner = User.Identity.Name;
            UserCollections = _collectionManager.GetCollectionsByOwner(owner);

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            Artwork = _artworkManager.GetArtworkById(id);
            if (Artwork == null) return RedirectToPage("/ArtworkOverview");

            if (SelectedCollectionId <= 0)
                return RedirectToPage(new { id });

            var collection = _collectionManager.GetCollectionById(SelectedCollectionId);
            if (collection == null)
                return RedirectToPage(new { id });

            bool alreadyExists = _collectionManager.ArtworkExistsInCollection(SelectedCollectionId, id);
            if (alreadyExists)
            {
                SuccessMessage = $"Artwork already exists in '{collection.Title}'.";
            }
            else
            {
                _collectionManager.AddArtworkToCollection(SelectedCollectionId, id);
                SuccessMessage = $"Artwork was successfully added to '{collection.Title}'.";
            }

            return RedirectToPage(new { id });
        }

    }
}
