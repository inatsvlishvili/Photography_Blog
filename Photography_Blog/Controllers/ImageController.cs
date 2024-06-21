using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Photography_Blog.Data;
using Photography_Blog.Models;
using Photography_Blog.ViewModels;

namespace Photography_Blog.Controllers
{
    public class ImageController : Controller
    {
        private readonly ILogger<ImageController> _logger;
        BlogContext _DbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageController(IWebHostEnvironment webHostEnvironment, ILogger<ImageController> logger, BlogContext DbContext)
        {
            _logger = logger;
            _DbContext = DbContext;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<IActionResult> Image(PhotoViewModel vm)
        {
            var Photo = await _DbContext.Photos.Include(x => x.Category).Select(model => new PhotoViewModel()
            {
                Id = model.Id,
                Title = model.Title,
                PhotoUrl = model.PhotoUrl,
                ImageName = model.ImageName,
                Description = model.Description,
                CategoryId = model.CategoryId,
                PhotographerId = model.PhotographerId,
                CreatedateTime = DateTime.Now,
                PhotoCategoryName = model.Category.Title,
                PhotographerName = model.Photographer.NickName,
                PhotoCategoryNameGEO = model.Category.TitleGEO

            }
            ).ToListAsync();

            return View(Photo);
        }

        public async Task<IActionResult> AddImage()
        {
            var Photographers = await _DbContext.Photographers.ToListAsync();
            ViewBag.Photographers = Photographers;

            var Category = await _DbContext.Categories.ToListAsync();
            ViewBag.Categories = Category;
            ModelState.Clear();
            return View();
        }
        [HttpPost]

        public IActionResult AddImage(PhotoViewModel vm)
        {
            var Photographers = _DbContext.Photographers.ToList();
            ViewBag.Photographers = Photographers;

            var Category = _DbContext.Categories.ToList();
            ViewBag.Categories = Category;

            Photo model = new Photo();

            if (!ModelState.IsValid)
            {
                return View();
            }


            if (vm.ImageFile == null)
            {
                ViewBag.Error = "აირჩიეთ სურათი";
                return View();
            }

            var cateName = _DbContext.Categories.SingleOrDefault(x => x.Id == vm.CategoryId);
            ViewBag.CateName = cateName.Title;

            var FileDic = "images/photo/" + cateName.Title;
            string imgPath = Path.Combine(_webHostEnvironment.WebRootPath, FileDic);
            if (!Directory.Exists(imgPath))
                Directory.CreateDirectory(imgPath);

            foreach (var file in vm.ImageFile)
            {
                var img = file.FileName;
                string imgext = Path.GetExtension(img);
                var imageNewFileName = Guid.NewGuid().ToString();
                imageNewFileName = imageNewFileName + imgext;
                var filePath = Path.Combine(imgPath, imageNewFileName);
                using (FileStream fs = System.IO.File.Create(filePath))

                {
                    file.CopyTo(fs);
                }

                vm.ImageName = imageNewFileName;
            }

            model.Title = vm.Title;
            model.PhotoUrl = vm.PhotoUrl;
            model.ImageName = vm.ImageName;
            model.Description = vm.Description;
            model.CategoryId = vm.CategoryId;
            model.PhotographerId = vm.PhotographerId;
            model.CreatedateTime = DateTime.Now;

            _DbContext.Photos.Add(model);
            _DbContext.SaveChanges();

            return RedirectToAction("Image");
        }
        public async Task<IActionResult> EditImage(int id)
        {
            var image = await _DbContext.Photos.Where(x => x.Id == id).Select(model => new PhotoViewModel()
            {

                Title = model.Title,
                PhotoUrl = model.PhotoUrl,
                ImageName = model.ImageName,
                Description = model.Description,
                CategoryId = model.CategoryId,
                PhotographerId = model.PhotographerId,
                CreatedateTime = DateTime.Now,
                PhotoCategoryName = model.Category.Title,
                PhotographerName = model.Photographer.NickName,
                PhotoCategoryNameGEO = model.Category.TitleGEO

            }).FirstOrDefaultAsync();

            var photographers = await _DbContext.Photographers.ToListAsync();
            ViewBag.photographers = photographers;

            var Categories = await _DbContext.Categories.ToListAsync();
            ViewBag.Categories = Categories;

            return View(image);

        }
        [HttpPost]
        public IActionResult EditImage(PhotoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var photographers = _DbContext.Photographers.ToList();
            ViewBag.photographers = photographers;

            var Categories = _DbContext.Categories.ToList();
            ViewBag.Categories = Categories;

            var image = _DbContext.Photos.Where(x => x.Id == vm.Id).FirstOrDefault();
            var oldImageName = image.ImageName;

            var cateName = _DbContext.Categories.SingleOrDefault(x => x.Id == vm.CategoryId);
            var FileDic = "images/photo" + cateName.Title;


            string imgPath = Path.Combine(_webHostEnvironment.WebRootPath, FileDic);
            if (!Directory.Exists(imgPath))
                Directory.CreateDirectory(imgPath);

            foreach (var file in vm.ImageFile)
            {
                var img = file.FileName;
                string imgext = Path.GetExtension(img);
                var imageNewFileName = Guid.NewGuid().ToString();
                imageNewFileName = imageNewFileName + imgext;
                var filePath = Path.Combine(imgPath, imageNewFileName);
                using (FileStream fs = System.IO.File.Create(filePath))

                {
                    file.CopyTo(fs);
                }

                vm.ImageName = imageNewFileName;
            }


            image.Title = vm.Title;
            image.PhotoUrl = vm.PhotoUrl;
            image.ImageName = vm.ImageName;
            image.Description = vm.Description;
            image.CategoryId = vm.CategoryId;
            image.PhotographerId = vm.PhotographerId;
            image.CreatedateTime = DateTime.Now;

            _DbContext.Photos.Update(image);
            _DbContext.SaveChanges();


            imgPath = Path.Combine(_webHostEnvironment.WebRootPath, FileDic);
            if (vm.ImageName != null)
            {
                System.IO.File.Delete(Path.Combine(imgPath, oldImageName));
            }

            return RedirectToAction("Image");
        }

        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _DbContext.Photos.Where(x => x.Id == id).Select(model => new PhotoViewModel()
            {
                Id = model.Id,
                Title = model.Title,
                PhotoUrl = model.PhotoUrl,
                ImageName = model.ImageName,
                Description = model.Description,
                CategoryId = model.CategoryId,
                PhotographerId = model.PhotographerId,
                CreatedateTime = DateTime.Now,
                PhotoCategoryName = model.Category.Title,
                PhotographerName = model.Photographer.NickName,
                PhotoCategoryNameGEO = model.Category.TitleGEO

            }).FirstOrDefaultAsync();

            return View(image);
        }

        [HttpPost]
        public IActionResult DeleteImage(PhotoViewModel vm)
        {

            if (ModelState.IsValid)
            {
                var image = _DbContext.Photos.Find(vm.Id);
                if (image == null)
                {
                    return NotFound();
                }

                _DbContext.Photos.Remove(image);
                _DbContext.SaveChanges();
                return RedirectToAction("Image");

            }

            return View(vm);
        }

    }

}

