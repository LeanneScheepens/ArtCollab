using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Models;

namespace Logic.Interfaces
{
    public interface ICommentRepository
    {
        void CreateComment(Comment comment);
        void DeleteComment(int id);
        void UpdateComment(Comment comment);
        List<Comment> GetComments(int artworkId);
    }
}
