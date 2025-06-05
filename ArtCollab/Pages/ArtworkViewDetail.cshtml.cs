using Logic.Models;
using Logic.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ArtCollab.Services;


namespace ArtCollab.Pages
{
    public class ArtworkEventDetailModel : PageModel
    {
        private readonly ArtworkManager _artworkManager;
        private readonly CollectionManager _collectionManager;
        private readonly CommentManager _commentManager;

        public ArtworkEventDetailModel(
            ArtworkManager artworkManager,
            CollectionManager collectionManager,
            CommentManager commentManager)
        {
            _artworkManager = artworkManager;
            _collectionManager = collectionManager;
            _commentManager = commentManager;
        }

        [TempData]
        public string? SuccessMessage { get; set; }

        public Artwork Artwork { get; set; }

        [BindProperty]
        public int SelectedCollectionId { get; set; }

        [BindProperty]
        public int CommentId { get; set; }

        [BindProperty]
        public string NewContent { get; set; }

        [BindProperty]
        public string NewCommentContent { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? EditCommentId { get; set; }

        public List<Logic.Models.Collection> UserCollections { get; set; } = new();

        public List<Comment> Comments { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            Artwork = _artworkManager.GetArtworkById(id);
            if (Artwork == null) return RedirectToPage("/ArtworkOverview");

            var owner = User.Identity?.Name;
            UserCollections = _collectionManager.GetCollectionsByOwner(owner);
            Comments = _commentManager.GetCommentsByArtworkId(id);

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

        public IActionResult OnPostAddComment(int id)
        {
            if (string.IsNullOrWhiteSpace(NewCommentContent))
                return RedirectToPage(new { id });

            var author = User.Identity?.Name ?? "Anonymous";
            var comment = new Comment(0, id, NewCommentContent, author, DateTime.Now);

            _commentManager.AddComment(comment);

            return RedirectToPage(new { id });
        }

        public IActionResult OnPostDeleteComment(int id, int commentId)
        {
            var user = User.Identity?.Name;
            var comment = _commentManager.GetCommentsByArtworkId(id).FirstOrDefault(c => c.Id == commentId);

            if (comment == null || comment.Author != user)
                return Forbid();

            _commentManager.DeleteComment(commentId);
            return RedirectToPage(new { id });
        }

        public IActionResult OnPostEditComment(int id, int commentId, string newContent)
        {
            var user = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(newContent))
                return RedirectToPage(new { id });

            var comment = _commentManager.GetCommentsByArtworkId(id).FirstOrDefault(c => c.Id == commentId);

            if (comment == null || comment.Author != user)
                return Forbid();

            comment.Content = newContent;
            comment.UploadDate = DateTime.Now;

            _commentManager.UpdateComment(comment);

            return RedirectToPage(new { id });
        }
    }
}