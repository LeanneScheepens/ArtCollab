
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Logic.Managers;

namespace ArtCollab.Pages.Collection
{
    public class CollectionOverviewModel : PageModel
    {
        private readonly CollectionManager _manager;

        public CollectionOverviewModel(CollectionManager manager)
        {
            _manager = manager;
        }

        public List<Logic.Models.Collection> Collections { get; set; } = new();


        public void OnGet()
        {
            var owner = User.Identity.Name; // of claim-based
            Collections = _manager.GetCollectionsByOwner(owner);
        }
        public IActionResult OnPostDelete(int id)
        {
            var owner = User.Identity.Name;
            _manager.DeleteCollection(id, owner); // met eigenaar-verificatie
            return RedirectToPage();
        }

    }
}

