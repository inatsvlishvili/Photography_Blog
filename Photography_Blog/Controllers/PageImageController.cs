using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Photography_Blog.Data;
using Photography_Blog.ViewModels;


namespace Photography_Blog.Controllers
{
    public class PageImageController : Controller
    {
        private readonly ILogger<PageImageController> _logger;
        BlogContext _DbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PageImageController(IWebHostEnvironment webHostEnvironment, ILogger<PageImageController> logger, BlogContext DbContext)
        {
            _logger = logger;
            _DbContext = DbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> PageImage()
        {
            var image = await _DbContext.PagePhotos.Include(x => x.PagePhotoCategory).Select(model => new PagePhotoViewModel()
            {
                Id = model.Id,
                Title = model.Title,
                PhotoUrl = model.PhotoUrl,
                ImageName = model.ImageName,
                Description = model.Description,
                PagePhotoCategoryId = model.PagePhotoCategoryId,
                PhotographerId = model.PhotographerId,
                CategoryId = model.CategoryId,
                PagePhotoCategoryName = model.PagePhotoCategory.Title,
                PagePhotoCategoryNameGEO = model.PagePhotoCategory.TitleGEO,
                PagePhotoPhotographerName = model.Photographer.NickName
            }
            ).ToListAsync();


            return View(image);
        }

        public async Task<IActionResult> AddPageImage()
        {
            var Categories = await _DbContext.Categories.ToListAsync();
            ViewBag.Categories = Categories;

            var photographers = await _DbContext.Photographers.ToListAsync();
            ViewBag.photographers = photographers;

            var pagePhotoCategories = await _DbContext.PagePhotoCategories.ToListAsync();
            ViewBag.pagePhotoCategories = pagePhotoCategories;


            return View();
        }
        [HttpPost]

        public IActionResult AddPageImage(PagePhotoViewModel pagevm)
        {
            var Categories = _DbContext.Categories.ToList();
            ViewBag.Categories = Categories;

            var photographers = _DbContext.Photographers.ToList();
            ViewBag.photographers = photographers;

            var pagePhotoCategories = _DbContext.PagePhotoCategories.ToList();
            ViewBag.pagePhotoCategories = pagePhotoCategories;

            PagePhoto model = new PagePhoto();

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (pagevm.ImageFile == null)
            {
                ViewBag.Error = "აირჩიეთ სურათი";
                return View();
            }

            var cateName = _DbContext.PagePhotoCategories.SingleOrDefault(x => x.Id == pagevm.PagePhotoCategoryId);

            var FileDic = "images/pageimages/" + cateName.Title;
            ViewBag.pageimage = cateName.Title;

            string imgPath = Path.Combine(_webHostEnvironment.WebRootPath, FileDic);
            if (!Directory.Exists(imgPath))
                Directory.CreateDirectory(imgPath);

            foreach (var file in pagevm.ImageFile)
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

                pagevm.ImageName = imageNewFileName;
            }

            model.Title = pagevm.Title;
            model.PhotoUrl = pagevm.PhotoUrl;
            model.ImageName = pagevm.ImageName;
            model.Description = pagevm.Description;
            model.PagePhotoCategoryId = pagevm.PagePhotoCategoryId;
            model.PhotographerId = pagevm.PhotographerId;
            model.CategoryId = pagevm.CategoryId;

            _DbContext.PagePhotos.Add(model);
            _DbContext.SaveChanges();

            return RedirectToAction("pageimage");
        }

        public async Task<IActionResult> EditPageImage(int id)
        {
            var EditPagePhoto = await _DbContext.PagePhotos.Where(x => x.Id == id).Select(model => new PagePhotoViewModel()
            {
                Title = model.Title,
                PhotoUrl = model.PhotoUrl,
                ImageName = model.ImageName,
                Description = model.Description,
                PagePhotoCategoryId = model.PagePhotoCategoryId,
                PhotographerId = model.PhotographerId,
                CategoryId = model.CategoryId,
                PagePhotoCategoryName = model.PagePhotoCategory.Title,
                PagePhotoCategoryNameGEO = model.PagePhotoCategory.TitleGEO,
                PagePhotoPhotographerName = model.Photographer.NickName

            }).FirstOrDefaultAsync();

            var Categories = await _DbContext.Categories.ToListAsync();
            ViewBag.Categories = Categories;

            var photographers = await _DbContext.Photographers.ToListAsync();
            ViewBag.photographers = photographers;

            var pagePhotoCategories = await _DbContext.PagePhotoCategories.ToListAsync();
            ViewBag.pagePhotoCategories = pagePhotoCategories;



            return View(EditPagePhoto);

        }
        [HttpPost]
        public IActionResult EditPageImage(PagePhotoViewModel vm)
        {
            var Categories = _DbContext.Categories.ToList();
            ViewBag.Categories = Categories;

            var photographers = _DbContext.Photographers.ToList();
            ViewBag.photographers = photographers;

            var pagePhotoCategories = _DbContext.PagePhotoCategories.ToList();
            ViewBag.pagePhotoCategories = pagePhotoCategories;

            PagePhoto model = new PagePhoto();

            if (!ModelState.IsValid)
            {
                return View();
            }
            var image = _DbContext.PagePhotos.Where(x => x.Id == vm.Id).FirstOrDefault();
            var oldImageName = image.ImageName;

            var cateName = _DbContext.PagePhotoCategories.SingleOrDefault(x => x.Id == vm.PagePhotoCategoryId);

            var FileDic = "images/pageimages/" + cateName.Title;
            ViewBag.pageimage = cateName.Title;

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
            model.PagePhotoCategoryId = vm.PagePhotoCategoryId;
            model.PhotographerId = vm.PhotographerId;
            model.CategoryId = vm.CategoryId;


            _DbContext.PagePhotos.Add(model);
            _DbContext.SaveChanges();

            imgPath = Path.Combine(_webHostEnvironment.WebRootPath, FileDic);
            if (vm.ImageName != null)
            {
                System.IO.File.Delete(Path.Combine(imgPath, oldImageName));
            }

            return RedirectToAction("pageimage");
        }

        public async Task<IActionResult> DeletePageImage(int id)
        {
            var pageimage = await _DbContext.PagePhotos.Where(x => x.Id == id).Select(model => new PagePhotoViewModel()
            {
                Id = model.Id,
                Title = model.Title,
                PhotoUrl = model.PhotoUrl,
                ImageName = model.ImageName,
                Description = model.Description,
                PagePhotoCategoryId = model.PagePhotoCategoryId,
                PhotographerId = model.PhotographerId,
                CategoryId = model.CategoryId,
                PagePhotoCategoryName = model.PagePhotoCategory.Title,
                PagePhotoCategoryNameGEO = model.PagePhotoCategory.TitleGEO,
                PagePhotoPhotographerName = model.Photographer.NickName


            }).FirstOrDefaultAsync();


            return View(pageimage);
        }

        [HttpPost]
        public IActionResult DeletePageImage(PagePhotoViewModel vm)
        {

            if (ModelState.IsValid)
            {
                var pageimage = _DbContext.PagePhotos.Find(vm.Id);
                if (pageimage == null)
                {
                    return NotFound();
                }

                _DbContext.PagePhotos.Remove(pageimage);
                _DbContext.SaveChangesAsync();
                return RedirectToAction("PageImage");

            }

            return View(vm);
        }
    }
}
