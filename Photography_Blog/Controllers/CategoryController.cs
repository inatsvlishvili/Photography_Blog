using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Management.Smo;
using Photography_Blog.Data;
using Photography_Blog.Models;
using Photography_Blog.ViewModels;

namespace Photography_Blog.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        BlogContext _DbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryController(IWebHostEnvironment webHostEnvironment, ILogger<CategoryController> logger, BlogContext DbContext)
        {
            _logger = logger;
            _DbContext = DbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Category()
        {
            var cat = await _DbContext.Categories.Select(model => new CategoryViewModel()
            {
                Id = model.Id,
                Title = model.Title,
                TitleGEO = model.TitleGEO,

            }).ToListAsync();

            return View(cat);
        }
        public async Task<IActionResult> AddCategory()
        {
            return View();

        }
        [HttpPost]
        public IActionResult AddCategory(CategoryViewModel cat)
        {

            Category model = new Category();
            model.Title = cat.Title;
            model.TitleGEO = cat.TitleGEO;

            _DbContext.Categories.Add(model);
            _DbContext.SaveChanges();
            return RedirectToAction("Category");

        }
        public async Task<IActionResult> EditCategory(int id)
        {
            var catedelete = await _DbContext.Categories.Where(x => x.Id == id).Select(x => new CategoryViewModel()
            {
                Id = x.Id,
                Title = x.Title,
                TitleGEO = x.TitleGEO,

            }).FirstOrDefaultAsync();

            return View(catedelete);
        }

        [HttpPost]
        public IActionResult EditCategory(CategoryViewModel catvm)
        {
            var category = _DbContext.Categories.Where(x => x.Id == catvm.Id).FirstOrDefault();
            category.Title = catvm.Title;
            category.TitleGEO = catvm.TitleGEO;
            _DbContext.Categories.Update(category);
            _DbContext.SaveChanges();

            return RedirectToAction("Category");
        }
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var input = await _DbContext.Categories.FindAsync(id);

            var cate = await _DbContext.Categories.Where(x => x.Id == id).Select(x => new CategoryViewModel()
            {
                Id = x.Id,
                Title = x.Title,
                TitleGEO = x.TitleGEO,
            }).FirstOrDefaultAsync();


            return View(cate);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteCategory(CategoryViewModel catvm)
        {
            if (ModelState.IsValid)
            {
                var category = await _DbContext.Categories.FindAsync(catvm.Id);
                if (category == null)
                {
                    return NotFound();
                }

                _DbContext.Categories.Remove(category);
                await _DbContext.SaveChangesAsync();
                return RedirectToAction("Category");
            }

            return View(catvm);
        }

        public async Task<IActionResult> PageCategory()
        {
            var pagecat = await _DbContext.PagePhotoCategories.Select(model => new PagePhotoCategoryViewModel()
            {
                Id = model.Id,
                Title = model.Title,
                TitleGEO = model.TitleGEO,

            }).ToListAsync();

            return View(pagecat);
        }
        public IActionResult AddPageCategory()
        {

            return View();

        }
        [HttpPost]
        public IActionResult AddPageCategory(PagePhotoCategoryViewModel pagecatvm)
        {
            ViewBag.pagecat = _DbContext.PagePhotoCategories.ToList();
            PagePhotoCategory model = new PagePhotoCategory();
            model.Title = pagecatvm.Title;
            model.TitleGEO = pagecatvm.TitleGEO;

            _DbContext.PagePhotoCategories.Add(model);
            _DbContext.SaveChanges();

            return RedirectToAction("PageCategory");

        }
        public async Task<IActionResult> EditPageCategory(int id)
        {
            var pagePhotoCategory = await _DbContext.PagePhotoCategories.Where(x => x.Id == id).Select(x => new PagePhotoCategoryViewModel()
            {
                Id = x.Id,
                Title = x.Title,
                TitleGEO = x.TitleGEO,
            }).FirstOrDefaultAsync();
            
            return View(pagePhotoCategory);

        }
        [HttpPost]
        public IActionResult EditPageCategory(PagePhotoCategoryViewModel pagecat)
        {

            var pagecatedit = _DbContext.PagePhotoCategories.Where(x => x.Id == pagecat.Id).FirstOrDefault();
            pagecatedit.Title = pagecat.Title;
            pagecatedit.TitleGEO = pagecat.TitleGEO;
            _DbContext.PagePhotoCategories.Update(pagecatedit);
            _DbContext.SaveChanges();

            return RedirectToAction("PageCategory");
        }

        public async Task<IActionResult> DeletePageCategory(int id)
        {
            var pagePhotoCategory = await _DbContext.PagePhotoCategories.Where(x => x.Id == id).Select(x => new PagePhotoCategoryViewModel()
            {
                Id = x.Id,
                Title = x.Title,
                TitleGEO = x.TitleGEO,
            }).FirstOrDefaultAsync();

            return View(pagePhotoCategory);

        }
        [HttpPost]
        public async Task<IActionResult> DeletePageCategory(PagePhotoCategoryViewModel pagecatvm)
        {
            if (ModelState.IsValid)
            {
                var pagecategory = await _DbContext.PagePhotoCategories.FindAsync(pagecatvm.Id);
                if (pagecategory == null)
                {
                    return NotFound();
                }

                _DbContext.PagePhotoCategories.Remove(pagecategory);
                await _DbContext.SaveChangesAsync();
                return RedirectToAction("PageCategory");
            }

            return View(pagecatvm);

        }

    }
}
