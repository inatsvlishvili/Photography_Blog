using System.ComponentModel.DataAnnotations.Schema;

namespace Photography_Blog.ViewModels
{
    public class AboutUsViewModel
    {
        public int Id { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
        public string Text4 { get; set; }
        public string Text5 { get; set; }
        public string Text6 { get; set; }
        public string Text7 { get; set; }
        public string BlockquoteText { get; set; }
        public string BlockquoteTitle { get; set; }
        public string ImageName { get; set; }
        [NotMapped]
        public List<IFormFile> ImageFile { set; get; }
    }
}
