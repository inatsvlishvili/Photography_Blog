using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using Photography_Blog.Models;
using Photography_Blog.ViewModels;



namespace Photography_Blog.Data
{
    public class BlogContext : IdentityDbContext<ApplicationUser>
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Photographer> Photographers { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<PagePhoto> PagePhotos { get; set; }
        public DbSet<PagePhotoCategory> PagePhotoCategories { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<AboutUs> AboutUs { get; set; }
        public DbSet<MainPageText> MainPageTexts { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Photography_Blog.ViewModels.AboutUsViewModel> AboutUsViewModel { get; set; }
        public DbSet<Photography_Blog.ViewModels.ContactViewModel> ContactViewModel { get; set; }

    }
}
