using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Photography_Blog.Data;
using Photography_Blog.Models;
using Photography_Blog.ViewModels;

namespace Photography_Blog.Controllers
{
    public class VideoController : Controller
    {
        private readonly ILogger<VideoController> _logger;
        BlogContext _DbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VideoController(IWebHostEnvironment webHostEnvironment, ILogger<VideoController> logger, BlogContext DbContext)
        {
            _logger = logger;
            _DbContext = DbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Video(VideoViewModel vm)
        {
            var video = await _DbContext.Videos.Include(x => x.Category).Select(model => new VideoViewModel()
            {
                Id = model.Id,
                Title = model.Title,
                VideoUrl = model.VideoUrl,
                ImageName = model.ImageName,
                Description = model.Description,
                CategoryId = model.CategoryId,
                CreatedateTime = DateTime.Now,
                PhotographerId = model.PhotographerId,
                PhotoCategoryName = model.Category.Title,
                PhotographerName = model.Photographer.NickName,
                PhotoCategoryNameGEO = model.Category.TitleGEO
            }
            ).ToListAsync();

            return View(video);
        }

        public async Task<IActionResult> AddVideo()
        {
            var photographers = await _DbContext.Photographers.ToListAsync();
            ViewBag.photographers = photographers;

            var Categories = await _DbContext.Categories.ToListAsync();
            ViewBag.Categories = Categories;
            ModelState.Clear();
            return View();
        }
        [HttpPost]

        public IActionResult AddVideo(VideoViewModel vm)
        {
            var photographers = _DbContext.Photographers.ToList();
            ViewBag.photographers = photographers;

            var Categories = _DbContext.Categories.ToList();
            ViewBag.Categories = Categories;

            Video model = new Video();

            if (!ModelState.IsValid)
            {
                return View();
            }

            var FileDic = "Images/VideoImage";
            string imgPath = Path.Combine(_webHostEnvironment.WebRootPath, FileDic);
            if (!Directory.Exists(imgPath))
                Directory.CreateDirectory(imgPath);

            var imagename = vm.ImageFile.FileName;
            string imgext = Path.GetExtension(imagename);
            var imagename1NewFileName = Guid.NewGuid().ToString();
            imagename1NewFileName = imagename1NewFileName + imgext;
            var filePathpersonal = Path.Combine(imgPath, imagename1NewFileName);
            using (FileStream fs = System.IO.File.Create(filePathpersonal))
            {
                vm.ImageFile.CopyTo(fs);
            }
            vm.ImageName = imagename1NewFileName;


            model.Title = vm.Title;
            model.VideoUrl = vm.VideoUrl;
            model.ImageName = vm.ImageName;
            model.Description = vm.Description;
            model.CategoryId = vm.CategoryId;
            model.PhotographerId = vm.PhotographerId;
            model.CreatedateTime = DateTime.Now;

            _DbContext.Videos.Add(model);
            _DbContext.SaveChanges();
            return RedirectToAction("Video");

        }
        public async Task<IActionResult> EditVideo(int id)
        {

            var Videos = await _DbContext.Videos.Where(x => x.Id == id).Select(model => new VideoViewModel()
            {
                Id = model.Id,
                Title = model.Title,
                VideoUrl = model.VideoUrl,
                ImageName = model.ImageName,
                Description = model.Description,
                CategoryId = model.CategoryId,
                PhotographerId = model.PhotographerId,
                PhotoCategoryName = model.Category.Title,
                PhotographerName = model.Photographer.NickName,
                PhotoCategoryNameGEO = model.Category.TitleGEO
            }
            ).FirstOrDefaultAsync();

            var photographers = await _DbContext.Photographers.ToListAsync();
            ViewBag.photographers = photographers;

            var Categories = await _DbContext.Categories.ToListAsync();
            ViewBag.Categories = Categories;


            return View(Videos);
        }
        [HttpPost]
        public IActionResult EditVideo(VideoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var Videos = _DbContext.Videos.Where(x => x.Id == vm.Id).FirstOrDefault();
            var OldImage = Videos.ImageName;

            var photographers = _DbContext.Photographers.ToList();
            ViewBag.photographers = photographers;

            var Categories = _DbContext.Categories.ToList();
            ViewBag.Categories = Categories;

            Video model = new Video();

            if (!ModelState.IsValid)
            {
                return View();
            }

            var FileDic = "Images/VideoImage";
            string imgPath = Path.Combine(_webHostEnvironment.WebRootPath, FileDic);
            if (!Directory.Exists(imgPath))
                Directory.CreateDirectory(imgPath);

            var imagename = vm.ImageFile.FileName;
            string imgext = Path.GetExtension(imagename);
            var imagename1NewFileName = Guid.NewGuid().ToString();
            imagename1NewFileName = imagename1NewFileName + imgext;
            var filePathpersonal = Path.Combine(imgPath, imagename1NewFileName);
            using (FileStream fs = System.IO.File.Create(filePathpersonal))
            {
                vm.ImageFile.CopyTo(fs);
            }
            vm.ImageName = imagename1NewFileName;


            model.Title = vm.Title;
            model.VideoUrl = vm.VideoUrl;
            model.ImageName = vm.ImageName;
            model.Description = vm.Description;
            model.CategoryId = vm.CategoryId;
            model.PhotographerId = vm.PhotographerId;
            model.CreatedateTime = DateTime.Now;


            _DbContext.Videos.Update(model);
            _DbContext.SaveChanges();

            imgPath = Path.Combine(_webHostEnvironment.WebRootPath, FileDic);
            if (vm.ImageName != null)
            {
                System.IO.File.Delete(Path.Combine(imgPath, OldImage));
            }


            return RedirectToAction("Video");
        }
        public async Task<IActionResult> DeleteVideo(int id)
        {
            var video = await _DbContext.Videos.Where(x => x.Id == id).Select(model => new VideoViewModel()
            {
                Id = model.Id,
                Title = model.Title,
                VideoUrl = model.VideoUrl,
                ImageName = model.ImageName,
                Description = model.Description,
                CategoryId = model.CategoryId,
                PhotographerId = model.PhotographerId,
                CreatedateTime = DateTime.Now,
                PhotoCategoryName = model.Category.Title,
                PhotographerName = model.Photographer.NickName,
                PhotoCategoryNameGEO = model.Category.TitleGEO

            }).FirstOrDefaultAsync();

            return View(video);
        }

        [HttpPost]
        public IActionResult DeleteVideo(VideoViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var video = _DbContext.Videos.Find(vm.Id);
                if (video == null)
                {
                    return NotFound();
                }

                _DbContext.Videos.Remove(video);
                _DbContext.SaveChanges();
                return RedirectToAction("Video");

            }

            return View(vm);
        }
    }
}
