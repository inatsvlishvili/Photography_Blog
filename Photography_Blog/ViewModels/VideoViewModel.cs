using Photography_Blog.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Photography_Blog.ViewModels
{
    public class VideoViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string VideoUrl { get; set; }
        public string ImageName { get; set; }
        [NotMapped]
        public IFormFile ImageFile { set; get; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int PhotographerId { get; set; }
        public Photographer Photographer { get; set; }
        public DateTime CreatedateTime { get; set; }
        public string PhotoCategoryName { get; set; }
        public string PhotoCategoryNameGEO { get; set; }
        public string PhotographerName { get; set; }
    }
}
