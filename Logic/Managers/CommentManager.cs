using Logic.Interfaces;
using Logic.Models;

namespace ArtCollab.Services
{
    public class CommentManager
    {
        private readonly ICommentRepository _repository;

        public CommentManager(ICommentRepository repository)
        {
            _repository = repository;
        }

        public void AddComment(Comment comment)
        {
            if (string.IsNullOrWhiteSpace(comment.Content))
                throw new ArgumentException("Comment content cannot be empty.");

            if (string.IsNullOrWhiteSpace(comment.Author))
                throw new ArgumentException("Comment author is required.");

            comment.UploadDate = DateTime.Now;
            _repository.CreateComment(comment);
        }

        public void UpdateComment(Comment comment)
        {
            if (comment.Id <= 0)
                throw new ArgumentException("Invalid comment ID.");

            _repository.UpdateComment(comment);
        }

        public void DeleteComment(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid comment ID.");

            _repository.DeleteComment(id);
        }

        public List<Comment> GetCommentsByArtworkId(int artworkId)
        {
            if (artworkId <= 0)
                return new List<Comment>();

            return _repository.GetComments(artworkId);
        }
    }
}