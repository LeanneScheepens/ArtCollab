using Logic.Models;
using ArtCollab.Services;
using Logic.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Logic.ViewModels;
using Logic.Managers;

namespace ArtCollab.Pages.Collection
{
    public class CreateCollectionModel : PageModel
    {
        private readonly CollectionManager _collectionManager;
        private readonly ArtworkManager _artworkManager;

        public CreateCollectionModel(CollectionManager collectionManager, ArtworkManager artworkManager)
        {
            _collectionManager = collectionManager;
            _artworkManager = artworkManager;
        }

        [BindProperty]
        public CreateCollectionViewModel CollectionVM { get; set; } = new();

        public List<Artwork> UserArtworks { get; set; } = new();

        public void OnGet()
        {
            var owner = User.Identity.Name;
            UserArtworks = _artworkManager.GetArtworksByOwner(owner);
        }

        public IActionResult OnPost()
        {
            var owner = User.Identity.Name;

            if (!ModelState.IsValid)
            {
                OnGet();
                return Page();
            }

            var newCollection = new Logic.Models.Collection
            {
                Title = CollectionVM.Title,
                Owner = owner,
                UploadDate = DateTime.Now
            };


            _collectionManager.CreateCollection(newCollection);

            foreach (var artworkId in CollectionVM.SelectedArtworkIds)
            {
                _collectionManager.AddArtworkToCollection(newCollection.Id, artworkId);
            }

            return RedirectToPage("/Collection/CollectionOverview");
        }
    }
}