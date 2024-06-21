using System.ComponentModel.DataAnnotations.Schema;

namespace Photography_Blog.ViewModels
{
    public class MainPageTextViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
        public string ImageName1 { get; set; }
        [NotMapped]
        public IFormFile ImageFile1 { set; get; }
        public string ImageName2 { get; set; }
        [NotMapped]
        public IFormFile ImageFile2 { set; get; }
        public string VideoUrl1 { get; set; }
        public string VideoUrl2 { get; set; }
    }
}
