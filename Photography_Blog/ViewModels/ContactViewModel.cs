using Photography_Blog.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Photography_Blog.ViewModels
{
    public class ContactViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string CommentTXT { get; set; }
        public DateTime CreatedateTime { get; set; }

    }
}
