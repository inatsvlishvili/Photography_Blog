using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Photography_Blog.Data;
using Photography_Blog.Models;
using Photography_Blog.ViewModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace Photography_Blog.Controllers
{
    public class AdminController : Controller
    {
        BlogContext _DbContext;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(IWebHostEnvironment webHostEnvironment, BlogContext DbContext, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _webHostEnvironment = webHostEnvironment;
            _DbContext = DbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Users()
        {
            var Users = _DbContext.ApplicationUsers.Select(model => new ApplicationUserViewModel()
            {

                FirstName = model.FirstName,
                LastName = model.LastName,
                NickName = model.NickName,
                Email = model.Email,
                Address = model.Address,
                ImageName = model.ImageName,
                CreatedateTime = model.CreatedateTime,

            }
            ).ToList();



            return View(Users);
            //var users = _userManager.Users;
            //return View(users);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (ModelState.IsValid)
            {

                var FileDic = "Images/Admin";
                string imgPath = Path.Combine(_webHostEnvironment.WebRootPath, FileDic);
                if (!Directory.Exists(imgPath))
                    Directory.CreateDirectory(imgPath);

                var imagename = vm.ImageFile.FileName;
                string imgext = Path.GetExtension(imagename);
                var imagenameNewFileName = Guid.NewGuid().ToString();
                imagenameNewFileName = imagenameNewFileName + imgext;
                var filePathpersonal = Path.Combine(imgPath, imagenameNewFileName);
                using (FileStream fs = System.IO.File.Create(filePathpersonal))
                {
                    //vm.ImageFile.CopyTo(fs);
                    vm.ImageFile.CopyTo(fs);
                }
                vm.ImageName = imagenameNewFileName;

                var Manager = new ApplicationUser
                {
                    UserName = vm.Email,
                    Email = vm.Email,
                    PhoneNumber = vm.PhoneNumber,
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    NickName = vm.NickName,
                    Address = vm.Address,
                    ImageName = imagenameNewFileName
                };

                var result = await _userManager.CreateAsync(Manager, vm.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(Manager, "Manager");
                    await _signInManager.SignInAsync(Manager, isPersistent: false);
                    return RedirectToAction("Index", "Admin");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }


        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(vm.Email, vm.Password, vm.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "მონაცემები არასწორია.");
                    return View(vm);
                }
            }

            return View(vm);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(AdminController.Index), "Admin");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,

            };

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Any())
            {
                var role = roles[0];
                model.Role = role;
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.Address = model.Address;
                user.ImageName = model.ImageName;

                var result = await _userManager.UpdateAsync(user);
                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var role in userRoles)
                {
                    await _userManager.RemoveFromRoleAsync(user, role);
                }
                await _userManager.AddToRoleAsync(user, model.Role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Users");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }
        public IActionResult DeleteUser()
        {

            return View();
        }

        public IActionResult ContactInfo()
        {

            return View();
        }


        public async Task<IActionResult> Index()
        {
            var ImageCount = await _DbContext.Photos.CountAsync();
            ViewBag.ImageCount = ImageCount;

            var VideoCount = await _DbContext.Videos.CountAsync();
            ViewBag.VideoCount = VideoCount;

            var photographercount = await _DbContext.Photographers.CountAsync();
            ViewBag.Photographercount = photographercount;

            var MessageCount = await _DbContext.Contacts.CountAsync();
            ViewBag.MessageCount = MessageCount;

            var User = await _DbContext.ApplicationUsers.ToListAsync();
            ViewBag.User = User;

            return View();
        }

    }
}
