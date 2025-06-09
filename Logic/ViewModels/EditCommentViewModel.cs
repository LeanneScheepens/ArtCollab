using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.ViewModels
{
    public class EditCommentViewModel
    {
        [Required]
        public int CommentId { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }

        public string Author { get; set; }

        public DateTime UploadDate { get; set; }
    }
}
