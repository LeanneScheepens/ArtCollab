using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Logic.ViewModels
{
    public class EditArtworkViewModel
    {
        public int ArtworkId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Owner is required")]
        public string Owner { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Upload Date is required")]
        [DataType(DataType.Date)]
        public DateTime UploadDate { get; set; }
    }
}
