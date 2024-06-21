using Photography_Blog.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Photography_Blog.ViewModels
{
    public class PagePhoto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PhotoUrl { get; set; }
        public string ImageName { get; set; }
        [NotMapped]
        public List<IFormFile> ImageFile { set; get; }
        public string Description { get; set; }
        public int PhotographerId { get; set; }
        public Photographer Photographer { get; set; }
        public int PagePhotoCategoryId { get; set; }
        public PagePhotoCategory PagePhotoCategory { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public DateTime CreatedateTime { get; set; }
    }
}
