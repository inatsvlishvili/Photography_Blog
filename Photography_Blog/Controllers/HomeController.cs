using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Photography_Blog.Data;
using Photography_Blog.Models;
using Photography_Blog.ViewModels;
using System.Diagnostics;


namespace Photography_Blog.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IWebHostEnvironment webHostEnvironment, ILogger<HomeController> logger, BlogContext DbContext) : base(DbContext)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult PhotographerPartial()
        {
            var photo = _DbContext.Photos.ToList();
            ViewBag.photo = photo;

            var category = _DbContext.Categories.ToList();
            ViewBag.category = category;

            return View();

        }
        public IActionResult PhotoPartial()
        {
            var photo = _DbContext.Photos.ToList();
            ViewBag.photo = photo;

            var category = _DbContext.Categories.ToList();
            ViewBag.category = category;

            return View();

        }
        public async Task<IActionResult> VideoPartial()
        {
            var video = await _DbContext.Videos.ToListAsync();
            ViewBag.video = video;

            var category = await _DbContext.Categories.ToListAsync();
            ViewBag.category = category;

            return View();

        }

        public async Task<IActionResult> Index()
        {
            var categoryimage = await _DbContext.PagePhotos.Where(x => x.PagePhotoCategory.Title == "MainPageInstagram").Take(18).ToListAsync();
            ViewBag.cateimage = categoryimage;

            var mainpagecover = await _DbContext.PagePhotos.Where(x => x.PagePhotoCategory.Title == "MainPageCover").OrderBy(r => Guid.NewGuid()).Take(18).ToListAsync();
            ViewBag.mainpagecover = mainpagecover;

            var mainpagecategory = await _DbContext.PagePhotos.Where(x => x.PagePhotoCategory.Title == "MainPageCategory").Where(y => y.Category.Title == "Couple").FirstOrDefaultAsync();
            ViewBag.mainpagecategory = mainpagecategory;

            var MainText = await _DbContext.MainPageTexts.SingleOrDefaultAsync();
            ViewBag.MainText = MainText;
            var offer = await _DbContext.Offers.SingleOrDefaultAsync();
            ViewBag.offer = offer;

            var photo = await _DbContext.Photos.ToListAsync();
            ViewBag.photo = photo;
            ViewBag.persons = await _DbContext.Photographers.ToListAsync();
            ViewBag.cat = await _DbContext.Categories.ToListAsync();
            return View();
        }
        public async Task<IActionResult> Service()
        {
            var ServiceCover = await _DbContext.PagePhotos.Where(x => x.PagePhotoCategory.Title == "ServiceCoverPhoto").Take(18).ToListAsync();
            ViewBag.ServiceCover = ServiceCover;

            var contactinfo = await _DbContext.ContactInfos.SingleOrDefaultAsync();
            ViewBag.contactinfo = contactinfo;
            var AboutUs = await _DbContext.AboutUs.SingleOrDefaultAsync();
            ViewBag.AboutUs = AboutUs;

            var service = await _DbContext.Services.ToListAsync();
            ViewBag.service = service;

            var photo = await _DbContext.Photos.ToListAsync();
            ViewBag.photo = photo;

            return View();
        }

        public string GetFormat()
        {
            return "dd/mm/yy";
        }

        [HttpPost]
        public async Task<IActionResult> Service(ContactViewModel contact)
        {
            var ServiceCover = await _DbContext.PagePhotos.Where(x => x.PagePhotoCategory.Title == "ServiceCoverPhoto").Take(18).ToListAsync();
            ViewBag.ServiceCover = ServiceCover;

            var contactinfo = await _DbContext.ContactInfos.SingleOrDefaultAsync();
            ViewBag.contactinfo = contactinfo;

            var AboutUs = await _DbContext.AboutUs.SingleOrDefaultAsync();
            ViewBag.AboutUs = AboutUs;

            var service = await _DbContext.Services.ToListAsync();
            ViewBag.service = service;

            Contact model = new Contact();

            if (!ModelState.IsValid)
            {
                return View();
            }

            model.Name = contact.Name;
            model.Phone = contact.Phone;
            model.Email = contact.Email;
            model.Subject = contact.Subject;
            model.CommentTXT = contact.CommentTXT;
            model.CreatedateTime = DateTime.Now;

            _DbContext.Contacts.Add(model);
            _DbContext.SaveChanges();


            var photo = _DbContext.Photos.ToList();
            ViewBag.photo = photo;

            return View();
        }

        public async Task<IActionResult> Photo()
        {
            var photo = await _DbContext.Photos.Include(x => x.Category).OrderBy(r => Guid.NewGuid()).Select(model => new PhotoViewModel()
            {
                Id = model.Id,
                Title = model.Title,
                PhotoUrl = model.PhotoUrl,
                ImageName = model.ImageName,
                Description = model.Description,
                PhotographerId = model.PhotographerId,
                PhotoCategoryName = model.Category.Title,


            }
          ).ToListAsync();

            var category = await _DbContext.Categories.ToListAsync();
            ViewBag.category = category;

            var otherphotostack = await _DbContext.PagePhotos.Where(x => x.PagePhotoCategory.Title == "OtherPhotoStack").ToListAsync();
            var photostack = await _DbContext.PagePhotos.Where(x => x.PagePhotoCategory.Title == "PhotoStack").ToListAsync();

            ViewBag.wedding = await _DbContext.Photos.Where(x => x.Category.Title == "wedding").ToListAsync();

            ViewBag.otherpagephoto = otherphotostack;
            ViewBag.pagephoto = photostack;
            ViewBag.photo = photo;
            ViewBag.person = await _DbContext.Photographers.ToListAsync();
            ViewBag.cat = await _DbContext.Categories.ToListAsync();
            return View();

        }

        public async Task<IActionResult> Video()
        {
            var video = await _DbContext.Videos.Include(x => x.Category).OrderBy(r => Guid.NewGuid()).Select(model => new VideoViewModel()
            {
                Id = model.Id,
                Title = model.Title,
                VideoUrl = model.VideoUrl,
                ImageName = model.ImageName,
                Description = model.Description,
                PhotographerId = model.PhotographerId,
                PhotoCategoryName = model.Category.Title,


            }
          ).ToListAsync();

            ViewBag.video = video;
            var category = await _DbContext.Categories.ToListAsync();
            ViewBag.category = category;

            var otherphotostack = await _DbContext.PagePhotos.Where(x => x.PagePhotoCategory.Title == "OtherPhotoStack").ToListAsync();
            var photostack = await _DbContext.PagePhotos.Where(x => x.PagePhotoCategory.Title == "PhotoStack").ToListAsync();

            ViewBag.wedding = await _DbContext.Photos.Where(x => x.Category.Title == "wedding").ToListAsync();

            ViewBag.otherpagephoto = otherphotostack;
            ViewBag.pagephoto = photostack;
            ViewBag.person = await _DbContext.Photographers.ToListAsync();
            ViewBag.cat = await _DbContext.Categories.ToListAsync();
            return View();
        }

        public async Task<IActionResult> Photographer(int id)
        {

            var photo = await _DbContext.Photos.Include(x => x.Photographer)
            .Where(x => x.PhotographerId == id)

            .Select(model => new PhotoViewModel
            {
                Id = model.Id,
                Title = model.Title,
                PhotoUrl = model.PhotoUrl,
                ImageName = model.ImageName,
                Description = model.Description,
                PhotographerId = model.PhotographerId,
                PhotoCategoryName = model.Category.Title,

            }).ToListAsync();

            ViewBag.photo = photo;

            var personalphoto = await _DbContext.Photographers.Where(x => x.Id == id).ToListAsync();
            ViewBag.personalphoto = personalphoto;

            var photografer = await _DbContext.Photographers.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Id == id);

            var category = await _DbContext.Categories.ToListAsync();
            ViewBag.category = category;

            return View();

        }

        public async Task<IActionResult> OurTeam()
        {
            var photographer = await _DbContext.Photographers.ToListAsync();
            ViewBag.cover = photographer;
            var categoryimage = await _DbContext.PagePhotos.Where(x => x.PagePhotoCategory.Title == "OurTeamInstagram").Take(18).ToListAsync();
            ViewBag.cateimage = categoryimage;

            return View(photographer);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
