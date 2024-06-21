using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using Photography_Blog.Data;
using Photography_Blog.Models;
using Photography_Blog.ViewModels;

namespace Photography_Blog.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ILogger<ServiceController> _logger;
        BlogContext _DbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ServiceController(IWebHostEnvironment webHostEnvironment, ILogger<ServiceController> logger, BlogContext DbContext)
        {
            _logger = logger;
            _DbContext = DbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Service()
        {
            var service = await _DbContext.Services.Select(model => new ServiceViewModel()
            {
                Id = model.Id,
                Title = model.Title,
                Package = model.Package,
                Price = model.Price,
                Description = model.Description,
                ImageName = model.ImageName,

            }).ToListAsync();

            return View(service);

        }
        [HttpGet]
        public IActionResult AddService()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddService(ServiceViewModel service)
        {

            Service model = new Service();

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (service.ImageFile == null)
            {
                ViewBag.Error = "აირჩიეთ სურათი";
                return View();
            }
            var FileDic = "images/service/";
            string imgPath = Path.Combine(_webHostEnvironment.WebRootPath, FileDic);
            if (!Directory.Exists(imgPath))
                Directory.CreateDirectory(imgPath);

            foreach (var file in service.ImageFile)
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

                service.ImageName = imageNewFileName;
            }

            model.Title = service.Title;
            model.Package = service.Package;
            model.Price = service.Price;
            model.Description = service.Description;
            model.ImageName = service.ImageName;

            _DbContext.Services.Add(model);
            _DbContext.SaveChanges();
            return RedirectToAction("Service");

        }
        [HttpGet]
        public async Task<IActionResult> EditService(int Id)
        {
            var viewmodel = await _DbContext.Services.Where(x => x.Id == Id).Select(x => new ServiceViewModel()
            {
                //Id = x.Id,
                Title = x.Title,
                Package = x.Package,
                Price = x.Price,
                Description = x.Description,
                ImageName = x.ImageName,

            }).FirstOrDefaultAsync();

            return View(viewmodel);
        }

        [HttpPost]
        public IActionResult EditService(ServiceViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var service = _DbContext.Services.Where(x => x.Id == vm.Id).FirstOrDefault();
            var oldimage = service.ImageName;
            var FileDic = "images/service/";
            string imgPath = Path.Combine(_webHostEnvironment.WebRootPath, FileDic);
            if (!Directory.Exists(imgPath))
                Directory.CreateDirectory(imgPath);
            if (vm.ImageFile != null)
            {
                foreach (var file in service.ImageFile)
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

                    service.ImageName = imageNewFileName;
                }
            }


            service.Title = service.Title;
            service.Package = service.Package;
            service.Price = service.Price;
            service.Description = service.Description;
            service.ImageName = service.ImageName;

            _DbContext.Services.Update(service);
            _DbContext.SaveChanges();

            imgPath = Path.Combine(_webHostEnvironment.WebRootPath, FileDic);
            if (vm.ImageName != null)
            {
                System.IO.File.Delete(Path.Combine(imgPath, oldimage));
            }

            return RedirectToAction("Service");
        }

        [HttpGet]
        public IActionResult DeleteService(int Id)
        {
            var input = _DbContext.Services.Find(Id);

            var service = _DbContext.Services.Where(x => x.Id == Id).Select(x => new ServiceViewModel()
            {
                Id = x.Id,
                Title = x.Title,
                Package = x.Package,
                Price = x.Price,
                Description = x.Description,
                ImageName = x.ImageName,
            }).FirstOrDefault();

            return View(service);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteService(ServiceViewModel vm)
        {

            if (ModelState.IsValid)
            {
                var service = await _DbContext.Services.FindAsync(vm.Id);
                if (service == null)
                {
                    return NotFound();
                }

                _DbContext.Services.Remove(service);
                await _DbContext.SaveChangesAsync();
                return RedirectToAction("Service");
            }

            return View(vm);
        }
    }
}
