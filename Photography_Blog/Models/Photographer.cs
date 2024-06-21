using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Photography_Blog.Models
{
    public class Photographer
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Experience { get; set; }
        public string ShortBio { get; set; }
        public string ImageName { get; set; }
        [NotMapped]
        public IFormFile PersonalImage { set; get; }
        public string PageImageName { get; set; }
        [NotMapped]
        public IFormFile PageImage { set; get; }
        public ICollection<Photo> Photos { get; set; }
        public DateTime CreatedateTime { get; set; }
    }
}
