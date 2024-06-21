using System.ComponentModel.DataAnnotations.Schema;

namespace Photography_Blog.ViewModels
{
    public class ServiceViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Package { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        [NotMapped]
        public List<IFormFile> ImageFile { set; get; }
    }
}
