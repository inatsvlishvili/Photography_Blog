using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Photography_Blog.Data;
using Photography_Blog.ViewModels;
using Photography_Blog.Models;

namespace Photography_Blog.Controllers
{
    public class PhotographerController : Controller
    {
        private readonly ILogger<PhotographerController> _logger;
        BlogContext _DbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PhotographerController(IWebHostEnvironment webHostEnvironment, ILogger<PhotographerController> logger, BlogContext DbContext)
        {
            _logger = logger;
            _DbContext = DbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Photographer()
        {
            var photographers = await _DbContext.Photographers.Select(model => new PhotographerViewModel()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                NickName = model.NickName,
                Age = model.Age,
                DateOfBirth = model.DateOfBirth,
                Experience = model.Experience,
                ShortBio = model.ShortBio,
                ImageName = model.ImageName,
                PageImageName = model.PageImageName,
                CreatedateTime = model.CreatedateTime,

            }
           ).ToListAsync();


            return View(photographers);
        }
        public IActionResult AddPhotographer()
        {

            ModelState.Clear();
            return View();

        }
        [HttpPost]
        public IActionResult AddPhotographer(PhotographerViewModel vm)
        {
            Photographer model = new Photographer();

            if (!ModelState.IsValid)
            {

                return View();
            }

            var FileDic = "Images/Photographer";

            string imgPath = Path.Combine(_webHostEnvironment.WebRootPath, FileDic);

            if (!Directory.Exists(imgPath))
                Directory.CreateDirectory(imgPath);

            var personalimg = vm.PersonalImage.FileName;
            string imgext = Path.GetExtension(personalimg);
            var personalimageNewFileName = Guid.NewGuid().ToString();
            personalimageNewFileName = personalimageNewFileName + imgext;
            var filePathpersonal = Path.Combine(imgPath, personalimageNewFileName);
            using (FileStream fs = System.IO.File.Create(filePathpersonal))
            {
                vm.PersonalImage.CopyTo(fs);
            }
            vm.ImageName = personalimageNewFileName;


            var pageimg = vm.PageImage.FileName;
            string imgext2 = Path.GetExtension(pageimg);
            var pageimageNewFileName = Guid.NewGuid().ToString();
            pageimageNewFileName = pageimageNewFileName + imgext2;
            var filePathpage = Path.Combine(imgPath, pageimageNewFileName);
            using (FileStream fs = System.IO.File.Create(filePathpage))
            {
                vm.PageImage.CopyTo(fs);
            }
            vm.PageImageName = pageimageNewFileName;

            model.FirstName = vm.FirstName;
            model.LastName = vm.LastName;
            model.NickName = vm.NickName;
            model.Age = vm.Age;
            model.DateOfBirth = vm.DateOfBirth;
            model.Experience = vm.Experience;
            model.ShortBio = vm.ShortBio;
            model.ImageName = vm.ImageName;
            model.PageImageName = vm.PageImageName;
            model.CreatedateTime = DateTime.Now;

            _DbContext.Photographers.Add(model);
            _DbContext.SaveChanges();

            return RedirectToAction("Photographer");
        }
        [HttpGet]
        public async Task<IActionResult> DeletePhotographer(int id)
        {
            //var input = await _DbContext.Photographers.FindAsync(id);
            var photographers = await _DbContext.Photographers.Where(x => x.Id == id).Select(model => new PhotographerViewModel()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                NickName = model.NickName,
                Age = model.Age,
                DateOfBirth = model.DateOfBirth,
                Experience = model.Experience,
                ShortBio = model.ShortBio,
                ImageName = model.ImageName,
                PageImageName = model.PageImageName

            }).FirstOrDefaultAsync();

            return View(photographers);
        }
        [HttpPost]
        public async Task<IActionResult> DeletePhotographer(PhotographerViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var photographers = await _DbContext.Photographers.FindAsync(vm.Id);
                if (photographers == null)
                {
                    return NotFound();
                }

                _DbContext.Photographers.Remove(photographers);
                await _DbContext.SaveChangesAsync();
                return RedirectToAction("Photographer");
            }

            return View(vm);
        }
        public async Task<IActionResult> EditPhotographer(int id)

        {
            var photographers = await _DbContext.Photographers.Where(x => x.Id == id).Select(x => new PhotographerViewModel()
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                NickName = x.NickName,
                Age = x.Age,
                DateOfBirth = x.DateOfBirth,
                Experience = x.Experience,
                ShortBio = x.ShortBio,
                ImageName = x.ImageName,
                PageImageName = x.PageImageName

            }).FirstOrDefaultAsync();

            return View(photographers);
        }

        [HttpPost]
        public async Task<IActionResult> EditPhotographer(PhotographerViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var photograp = await _DbContext.Photographers.Where(x => x.Id == vm.Id).FirstOrDefaultAsync();
            var oldPageImageName = photograp.PageImageName;
            var oldImageName = photograp.ImageName;

            var FileDic = "Images/Photographer";
            string imgPath = Path.Combine(_webHostEnvironment.WebRootPath, FileDic);

            if (!Directory.Exists(imgPath))
                Directory.CreateDirectory(imgPath);

            var personalimg = vm.PersonalImage.FileName;
            string imgext = Path.GetExtension(personalimg);
            var personalimageNewFileName = Guid.NewGuid().ToString();
            personalimageNewFileName = personalimageNewFileName + imgext;
            var filePathpersonal = Path.Combine(imgPath, personalimageNewFileName);
            using (FileStream fs = System.IO.File.Create(filePathpersonal))
            {
                vm.PersonalImage.CopyTo(fs);
            }
            vm.ImageName = personalimageNewFileName;


            var pageimg = vm.PageImage.FileName;
            string imgext2 = Path.GetExtension(pageimg);
            var pageimageNewFileName = Guid.NewGuid().ToString();
            pageimageNewFileName = pageimageNewFileName + imgext2;
            var filePathpage = Path.Combine(imgPath, pageimageNewFileName);
            using (FileStream fs = System.IO.File.Create(filePathpage))
            {
                vm.PageImage.CopyTo(fs);
            }
            vm.PageImageName = pageimageNewFileName;

            photograp.FirstName = vm.FirstName;
            photograp.LastName = vm.LastName;
            photograp.NickName = vm.NickName;
            photograp.Age = vm.Age;
            photograp.DateOfBirth = vm.DateOfBirth;
            photograp.Experience = vm.Experience;
            photograp.ShortBio = vm.ShortBio;
            photograp.ImageName = vm.ImageName;
            photograp.PageImageName = vm.PageImageName;
            photograp.CreatedateTime = DateTime.Now;

            _DbContext.Photographers.Update(photograp);
            _DbContext.SaveChanges();


            imgPath = Path.Combine(_webHostEnvironment.WebRootPath, FileDic);
            if (vm.PersonalImage != null)
            {
                System.IO.File.Delete(Path.Combine(imgPath, oldPageImageName));
            }
            if (vm.PageImage != null)
                System.IO.File.Delete(Path.Combine(imgPath, oldImageName));

            return RedirectToAction("Photographer");
        }
    }
}




