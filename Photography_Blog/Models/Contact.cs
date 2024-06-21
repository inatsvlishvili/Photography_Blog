namespace Photography_Blog.Models
{
    public class Contact
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
