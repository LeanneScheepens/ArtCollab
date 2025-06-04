using Logic.Models;
using Microsoft.Data.SqlClient;
using Logic.Interfaces;
using Logic.Managers;


namespace Data
{
    public class CommentRepository : ICommentRepository
    {
        private readonly string _connectionString;

        public CommentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateComment(Comment comment)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand(@"
                INSERT INTO Comment ([content], [uploadDate], [author], [artworkId])
                VALUES (@Content, @UploadDate, @Author, @ArtworkId);
                SELECT SCOPE_IDENTITY();", conn);

            cmd.Parameters.AddWithValue("@Content", comment.Content);
            cmd.Parameters.AddWithValue("@UploadDate", comment.UploadDate);
            cmd.Parameters.AddWithValue("@Author", comment.Author);
            cmd.Parameters.AddWithValue("@ArtworkId", comment.ArtworkId);

            comment.Id = Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void DeleteComment(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand("DELETE FROM Comment WHERE id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
        }

        public void UpdateComment(Comment comment)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand(@"
                UPDATE Comment 
                SET [content] = @Content, 
                    [uploadDate] = @UploadDate, 
                    [author] = @Author,
                    [artworkId] = @ArtworkId
                WHERE [id] = @Id", conn);

            cmd.Parameters.AddWithValue("@Content", comment.Content);
            cmd.Parameters.AddWithValue("@UploadDate", comment.UploadDate);
            cmd.Parameters.AddWithValue("@Author", comment.Author);
            cmd.Parameters.AddWithValue("@ArtworkId", comment.ArtworkId);

            cmd.ExecuteNonQuery();
        }

        public List<Comment> GetComments(int artworkId)
        {
            var comments = new List<Comment>();

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand(@"
                SELECT [id], [content], [author], [uploadDate], [artworkId]
                FROM Comment
                WHERE [artworkId] = @ArtworkId
                ORDER BY [uploadDate] DESC", conn);

            cmd.Parameters.AddWithValue("@ArtworkId", artworkId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var comment = new Comment(
                    id: reader.GetInt32(0),
                    content: reader.GetString(1),
                    author: reader.GetString(2),
                    uploadDate: reader.GetDateTime(3),
                    artworkId: reader.GetInt32(4)
                );

                comments.Add(comment);
            }

            return comments;
        }
    }
}