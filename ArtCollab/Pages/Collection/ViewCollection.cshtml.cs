using Logic.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArtCollab.Pages.Collection
{
    public class ViewCollectionModel : PageModel
    {
        private readonly CollectionManager _collectionManager;

        public ViewCollectionModel(CollectionManager collectionManager)
        {
            _collectionManager = collectionManager;
        }

        public Logic.Models.Collection Collection { get; set; }


        public IActionResult OnGet(int id)
        {
            Collection = _collectionManager.GetCollectionById(id);
            if (Collection == null) return NotFound();
            return Page();
        }
    }
}
