using System.ComponentModel.DataAnnotations.Schema;

namespace Photography_Blog.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PhotoUrl { get; set; }
        public string ImageName { get; set; }
        [NotMapped]
        public List<IFormFile> ImageFile { set; get; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int PhotographerId { get; set; }
        public Photographer Photographer { get; set; }
        public DateTime CreatedateTime { get; set; }
    }
}
