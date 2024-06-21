using Photography_Blog.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Photography_Blog.ViewModels
{
    public class PagePhotoViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PhotoUrl { get; set; }
        public string ImageName { get; set; }
        [NotMapped]
        public List<IFormFile> ImageFile { set; get; }
        public string Description { get; set; }
        public int PhotographerId { get; set; }
        public int PagePhotoCategoryId { get; set; }
        public string PagePhotoCategoryName { get; set; }
        public string PagePhotoCategoryNameGEO { get; set; }
        public string PagePhotoPhotographerName { get; set; }
        public string PagePhotoCategory { get; set; }
        public int CategoryId { get; set; }

    }
}
