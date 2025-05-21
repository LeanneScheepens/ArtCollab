using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace Logic.ViewModels
{
    public class CreateArtworkViewModel
    {

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime UploadDate { get; set; }

        [Required(ErrorMessage = ".png Image is required")]
        public IFormFile ImageFile { get; set; }


    }
}
