using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Photography_Blog.Data;
using Photography_Blog.Models;
using Photography_Blog.ViewModels;



namespace Photography_Blog.Controllers
{
    public class ContactInfoController : Controller
    {
        private readonly ILogger<ContactInfoController> _logger;
        BlogContext _DbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ContactInfoController(IWebHostEnvironment webHostEnvironment, ILogger<ContactInfoController> logger, BlogContext DbContext)
        {
            _logger = logger;
            _DbContext = DbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> ContactInfo()
        {
            var contactinfo = await _DbContext.ContactInfos.Select(model => new ContactInfoViewModel()
            {
                Id = model.Id,
                Title = model.Title,
                Address = model.Address,
                Phone = model.Phone,
                Email = model.Email,
                Map = model.Map,
                Facebook = model.Facebook,
                Instagram = model.Instagram,
                Tiktok = model.Tiktok,
                Youtube = model.Youtube,
                Twitter = model.Twitter,
                Linkedin = model.Linkedin

            }).ToListAsync();

            return View(contactinfo);
        }
        public IActionResult AddContactInfo()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddContactInfo(ContactInfoViewModel contactinfo)
        {
            ContactInfo model = new ContactInfo();
            model.Title = contactinfo.Title;
            model.Address = contactinfo.Address;
            model.Phone = contactinfo.Phone;
            model.Email = contactinfo.Email;
            model.Map = contactinfo.Map;
            model.Facebook = contactinfo.Facebook;
            model.Instagram = contactinfo.Instagram;
            model.Tiktok = contactinfo.Tiktok;
            model.Youtube = contactinfo.Youtube;
            model.Twitter = contactinfo.Twitter;
            model.Linkedin = contactinfo.Linkedin;


            _DbContext.ContactInfos.Add(model);
            _DbContext.SaveChanges();
            return RedirectToAction("ContactInfo");
        }
        public async Task<IActionResult> EditContactInfo(int id)
        {
            var contactinfo = await _DbContext.ContactInfos.Where(x => x.Id == id).Select(x => new ContactInfoViewModel()
            {
                Id = x.Id,
                Title = x.Title,
                Address = x.Address,
                Phone = x.Phone,
                Email = x.Email,
                Map = x.Map,
                Facebook = x.Facebook,
                Instagram = x.Instagram,
                Tiktok = x.Tiktok,
                Youtube = x.Youtube,
                Twitter = x.Twitter,
                Linkedin = x.Linkedin

            }).FirstOrDefaultAsync();


            return View(contactinfo);
        }
        [HttpPost]
        public IActionResult EditContactInfo(ContactInfoViewModel vm)
        {
            var contactinfo = _DbContext.ContactInfos.Where(x => x.Id == vm.Id).FirstOrDefault();
            contactinfo.Title = vm.Title;
            contactinfo.Address = vm.Address;
            contactinfo.Phone = vm.Phone;
            contactinfo.Email = vm.Email;
            contactinfo.Map = vm.Map;
            contactinfo.Facebook = vm.Facebook;
            contactinfo.Instagram = vm.Instagram;
            contactinfo.Tiktok = vm.Tiktok;
            contactinfo.Youtube = vm.Youtube;
            contactinfo.Twitter = vm.Twitter;
            contactinfo.Linkedin = vm.Linkedin;

            _DbContext.ContactInfos.Update(contactinfo);
            _DbContext.SaveChanges();

            return RedirectToAction("ContactInfo");
        }
        public async Task<IActionResult> DeleteContactInfo(int id)
        {

            var contactinfo = await _DbContext.ContactInfos.Where(x => x.Id == id).Select(x => new ContactInfoViewModel()
            {
                Id = x.Id,
                Title = x.Title,
                Address = x.Address,
                Phone = x.Phone,
                Email = x.Email,
                Map = x.Map,
                Facebook = x.Facebook,
                Instagram = x.Instagram,
                Tiktok = x.Tiktok,
                Youtube = x.Youtube,
                Twitter = x.Twitter,
                Linkedin = x.Linkedin
            }).FirstOrDefaultAsync();


            return View(contactinfo);
        }
        [HttpPost]
        public IActionResult DeleteContactInfo(ContactInfoViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var contactinfo = _DbContext.ContactInfos.Find(vm.Id);
                if (contactinfo == null)
                {
                    return NotFound();
                }

                _DbContext.ContactInfos.Remove(contactinfo);
                _DbContext.SaveChangesAsync();
                return RedirectToAction("ContactInfo");
            }

            return View(vm);
        }


        public async Task<IActionResult> MainPageText()
        {
            var MainPageText = await _DbContext.MainPageTexts.Select(model => new MainPageTextViewModel()
            {
                Id = model.Id,
                Title = model.Title,
                Text1 = model.Text1,
                Text2 = model.Text2,
                Text3 = model.Text3,
                ImageName1 = model.ImageName1,
                ImageName2 = model.ImageName2,
                VideoUrl1 = model.VideoUrl1,
                VideoUrl2 = model.VideoUrl2


            }).ToListAsync();

            return View(MainPageText);
        }
        public IActionResult AddMainPageText()
        {
            ModelState.Clear();
            return View();


        }
        [HttpPost]
        public IActionResult AddMainPageText(MainPageTextViewModel vm)
        {
            MainPageText model = new MainPageText();

            if (!ModelState.IsValid)
            {

                return View();
            }


            var FileDic = "Images/MainPageText";
            string imgPath = Path.Combine(_webHostEnvironment.WebRootPath, FileDic);
            if (!Directory.Exists(imgPath))
                Directory.CreateDirectory(imgPath);

            var imagename1 = vm.ImageFile1.FileName;
            string imgext = Path.GetExtension(imagename1);
            var imagename1NewFileName = Guid.NewGuid().ToString();
            imagename1NewFileName = imagename1NewFileName + imgext;
            var filePathpersonal = Path.Combine(imgPath, imagename1NewFileName);
            using (FileStream fs = System.IO.File.Create(filePathpersonal))
            {
                vm.ImageFile1.CopyTo(fs);
            }
            vm.ImageName1 = imagename1NewFileName;


            var imagename2 = vm.ImageFile2.FileName;
            string imgext2 = Path.GetExtension(imagename2);
            var imagename2NewFileName = Guid.NewGuid().ToString();
            imagename2NewFileName = imagename2NewFileName + imgext2;
            var filePathpage = Path.Combine(imgPath, imagename2NewFileName);
            using (FileStream fs = System.IO.File.Create(filePathpage))
            {
                vm.ImageFile2.CopyTo(fs);
            }
            vm.ImageName2 = imagename2NewFileName;


            model.Title = vm.Title;
            model.Text1 = vm.Text1;
            model.Text2 = vm.Text2;
            model.Text3 = vm.Text3;
            model.ImageName1 = vm.ImageName1;
            model.ImageName2 = vm.ImageName2;
            model.VideoUrl1 = vm.VideoUrl1;
            model.VideoUrl2 = vm.VideoUrl2;

            _DbContext.MainPageTexts.Add(model);
            _DbContext.SaveChanges();

            return RedirectToAction("MainPageText");
        }

        public async Task<IActionResult> EditMainPageText(int id)
        {
            var mainpagetxt = await _DbContext.MainPageTexts.Where(x => x.Id == id).Select(x => new MainPageTextViewModel()
            {
                Title = x.Title,
                Text1 = x.Text1,
                Text2 = x.Text2,
                Text3 = x.Text3,
                ImageName1 = x.ImageName1,
                ImageName2 = x.ImageName2,
                VideoUrl1 = x.VideoUrl1,
                VideoUrl2 = x.VideoUrl2

            }).FirstOrDefaultAsync();


            return View(mainpagetxt);
        }
        [HttpPost]
        public IActionResult EditMainPageText(MainPageTextViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var pagetext = _DbContext.MainPageTexts.Where(x => x.Id == vm.Id).FirstOrDefault();
            var oldImageName1 = pagetext.ImageName1;
            var oldImageName2 = pagetext.ImageName2;



            var FileDic = "Images/MainPageText";
            string imgPath = Path.Combine(_webHostEnvironment.WebRootPath, FileDic);
            if (!Directory.Exists(imgPath))
                Directory.CreateDirectory(imgPath);

            var imagename1 = vm.ImageFile1.FileName;
            string imgext = Path.GetExtension(imagename1);
            var imagename1NewFileName = Guid.NewGuid().ToString();
            imagename1NewFileName = imagename1NewFileName + imgext;
            var filePathpersonal = Path.Combine(imgPath, imagename1NewFileName);
            using (FileStream fs = System.IO.File.Create(filePathpersonal))
            {
                vm.ImageFile1.CopyTo(fs);
            }
            vm.ImageName1 = imagename1NewFileName;


            var imagename2 = vm.ImageFile2.FileName;
            string imgext2 = Path.GetExtension(imagename2);
            var imagename2NewFileName = Guid.NewGuid().ToString();
            imagename2NewFileName = imagename2NewFileName + imgext2;
            var filePathpage = Path.Combine(imgPath, imagename2NewFileName);
            using (FileStream fs = System.IO.File.Create(filePathpage))
            {
                vm.ImageFile2.CopyTo(fs);
            }
            vm.ImageName2 = imagename2NewFileName;

            pagetext.Title = vm.Title;
            pagetext.Text1 = vm.Text1;
            pagetext.Text2 = vm.Text2;
            pagetext.Text3 = vm.Text3;
            pagetext.ImageName1 = vm.ImageName1;
            pagetext.ImageName2 = vm.ImageName2;
            pagetext.VideoUrl1 = vm.VideoUrl1;
            pagetext.VideoUrl2 = vm.VideoUrl2;

            _DbContext.MainPageTexts.Update(pagetext);
            _DbContext.SaveChanges();

            return RedirectToAction("MainPageText");

        }


        public async Task<IActionResult> DeleteMainPageText(int id)
        {
            var mainpagetext = await _DbContext.MainPageTexts.Where(x => x.Id == id).Select(x => new MainPageTextViewModel()
            {
                Id = x.Id,
                Title = x.Title,
                Text1 = x.Text1,
                Text2 = x.Text2,
                Text3 = x.Text3,
                ImageName1 = x.ImageName1,
                ImageName2 = x.ImageName2,
                VideoUrl1 = x.VideoUrl1,
                VideoUrl2 = x.VideoUrl2

            }).FirstOrDefaultAsync();


            return View(mainpagetext);
        }

        [HttpPost]
        public IActionResult DeleteMainPageText(MainPageTextViewModel vm)
        {

            if (ModelState.IsValid)
            {
                var mainpagetext = _DbContext.MainPageTexts.Find(vm.Id);
                if (mainpagetext == null)
                {
                    return NotFound();
                }

                _DbContext.MainPageTexts.Remove(mainpagetext);
                _DbContext.SaveChangesAsync();
                return RedirectToAction("MainPageText");
            }


            return View(vm);
        }

        public async Task<IActionResult> Offer()
        {
            var offer = await _DbContext.Offers.Select(model => new OfferViewModel()
            {
                Id = model.Id,
                Title1 = model.Title1,
                Text1 = model.Text1,
                Title2 = model.Title2,
                Text2 = model.Text2,
                Title3 = model.Title3,
                Text3 = model.Text3,

            }).ToListAsync();

            return View(offer);
        }
        public IActionResult AddOffer()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddOffer(OfferViewModel offer)
        {
            Offer model = new Offer();

            model.Title1 = offer.Title1;
            model.Text1 = offer.Text1;
            model.Title2 = offer.Title2;
            model.Text2 = offer.Text2;
            model.Title3 = offer.Title3;
            model.Text3 = offer.Text3;

            _DbContext.Offers.Add(model);
            _DbContext.SaveChanges();
            return RedirectToAction("Offer");
        }
        public async Task<IActionResult> EditOffer(int id)
        {

            var offer = await _DbContext.Offers.Where(x => x.Id == id).Select(model => new OfferViewModel()
            {
                Title1 = model.Title1,
                Text1 = model.Text1,
                Title2 = model.Title2,
                Text2 = model.Text2,
                Title3 = model.Title3,
                Text3 = model.Text3

            }).FirstOrDefaultAsync();

            return View(offer);
        }
        [HttpPost]
        public IActionResult EditOffer(OfferViewModel vm)
        {
            var offer = _DbContext.Offers.Where(x => x.Id == vm.Id).FirstOrDefault();

            offer.Title1 = vm.Title1;
            offer.Text1 = vm.Text1;
            offer.Title2 = vm.Title2;
            offer.Text2 = vm.Text2;
            offer.Title3 = vm.Title3;
            offer.Text3 = vm.Text3;

            _DbContext.Offers.Update(offer);
            _DbContext.SaveChanges();

            return RedirectToAction("Offer");
        }
        public async Task<IActionResult> DeleteOffer(int id)
        {
            var offer = await _DbContext.Offers.Where(x => x.Id == id).Select(model => new OfferViewModel()
            {
                Title1 = model.Title1,
                Text1 = model.Text1,
                Title2 = model.Title2,
                Text2 = model.Text2,
                Title3 = model.Title3,
                Text3 = model.Text3

            }).FirstOrDefaultAsync();


            return View(offer);
        }
        [HttpPost]
        public IActionResult DeleteOffer(OfferViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var offer = _DbContext.Offers.Find(vm.Id);
                if (offer == null)
                {
                    return NotFound();
                }

                _DbContext.Offers.Remove(offer);
                _DbContext.SaveChangesAsync();
                return RedirectToAction("Offer");
            }

            return View(vm);
        }

        public async Task<IActionResult> Message()
        {
            var Message = await _DbContext.Contacts.Select(model => new ContactViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                Phone = model.Phone,
                Email = model.Email,
                Subject = model.Subject,
                CommentTXT = model.CommentTXT,
                CreatedateTime = model.CreatedateTime
            }).ToListAsync();

            return View(Message);

        }

        public async Task<IActionResult> EditMessage(int id)
        {

            var Message = await _DbContext.Contacts.Where(x => x.Id == id).Select(model => new ContactViewModel()
            {
                Name = model.Name,
                Phone = model.Phone,
                Email = model.Email,
                Subject = model.Subject,
                CommentTXT = model.CommentTXT,
              

            }).FirstOrDefaultAsync();

            return View(Message);
        }
        [HttpPost]
        public IActionResult EditMessage(ContactViewModel vm)
        {
            var Message = _DbContext.Contacts.Where(x => x.Id == vm.Id).FirstOrDefault();

            Message.Name = vm.Name;
            Message.Phone = vm.Phone;
            Message.Email = vm.Email;
            Message.Subject = vm.Subject;
            Message.CommentTXT = vm.CommentTXT;
            Message.CreatedateTime = DateTime.Now;

            _DbContext.Contacts.Update(Message);
            _DbContext.SaveChanges();

            return RedirectToAction("Message");
        }

        public async Task<IActionResult> DeleteMessage(int id)
        {
            var Message = await _DbContext.Contacts.Where(x => x.Id == id).Select(model => new ContactViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                Phone = model.Phone,
                Email = model.Email,
                Subject = model.Subject,
                CommentTXT = model.CommentTXT,
                CreatedateTime = model.CreatedateTime
            }).FirstOrDefaultAsync();



            return View(Message);

        }
        [HttpPost]
        public IActionResult DeleteMessage(ContactViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var contactinfo = _DbContext.Contacts.Find(vm.Id);
                if (contactinfo == null)
                {
                    return NotFound();
                }

                _DbContext.Contacts.Remove(contactinfo);
                _DbContext.SaveChangesAsync();
                return RedirectToAction("Message");
            }

            return View(vm);

        }

        public async Task<IActionResult> AboutUs()
        {

            var AboutUs = await _DbContext.AboutUs.Select(model => new AboutUsViewModel()
            {
                Id = model.Id,
                Text1 = model.Text1,
                Text2 = model.Text2,
                Text3 = model.Text3,
                Text4 = model.Text4,
                Text5 = model.Text5,
                Text6 = model.Text6,
                Text7 = model.Text7,
                BlockquoteText = model.BlockquoteText,
                BlockquoteTitle = model.BlockquoteTitle,

            }).ToListAsync();

            return View(AboutUs);
        }

        public IActionResult AddAboutUs()
        {

            return View();
        }
        [HttpPost]
        public IActionResult AddAboutUs(AboutUsViewModel vm)
        {

            AboutUs model = new AboutUs();

            model.Text1 = vm.Text1;
            model.Text2 = vm.Text2;
            model.Text3 = vm.Text3;
            model.Text4 = vm.Text4;
            model.Text5 = vm.Text5;
            model.Text6 = vm.Text6;
            model.Text7 = vm.Text7;
            model.BlockquoteText = vm.BlockquoteText;
            model.BlockquoteTitle = vm.BlockquoteTitle;


            _DbContext.AboutUs.Add(model);
            _DbContext.SaveChanges();
            return RedirectToAction("AboutUs");
        }

        public async Task<IActionResult> EditAboutUs(int id)
        {
            var AboutUs = await _DbContext.AboutUs.Where(x => x.Id == id).Select(x => new AboutUsViewModel()
            {
                Id = x.Id,
                Text1 = x.Text1,
                Text2 = x.Text2,
                Text3 = x.Text3,
                Text4 = x.Text4,
                Text5 = x.Text5,
                Text6 = x.Text6,
                Text7 = x.Text7,
                BlockquoteText = x.BlockquoteText,
                BlockquoteTitle = x.BlockquoteTitle

            }).FirstOrDefaultAsync();

            return View(AboutUs);
        }
        [HttpPost]
        public IActionResult EditAboutUs(AboutUsViewModel vm)
        {
            var AboutUs = _DbContext.AboutUs.Where(x => x.Id == vm.Id).FirstOrDefault();

            AboutUs.Text1 = vm.Text1;
            AboutUs.Text2 = vm.Text2;
            AboutUs.Text3 = vm.Text3;
            AboutUs.Text4 = vm.Text4;
            AboutUs.Text5 = vm.Text5;
            AboutUs.Text6 = vm.Text6;
            AboutUs.Text7 = vm.Text7;
            AboutUs.BlockquoteText = vm.BlockquoteText;
            AboutUs.BlockquoteTitle = vm.BlockquoteTitle;

            _DbContext.AboutUs.Update(AboutUs);
            _DbContext.SaveChanges();

            return RedirectToAction("AboutUs");
        }

        public async Task<IActionResult> DeleteAboutUs(int id)
        {

            var AboutUs = await _DbContext.AboutUs.Where(x => x.Id == id).Select(x => new AboutUsViewModel()
            {
                Id = x.Id,
                Text1 = x.Text1,
                Text2 = x.Text2,
                Text3 = x.Text3,
                Text4 = x.Text4,
                Text5 = x.Text5,
                Text6 = x.Text6,
                Text7 = x.Text7,
                BlockquoteText = x.BlockquoteText,
                BlockquoteTitle = x.BlockquoteTitle

            }).FirstOrDefaultAsync();


            return View(AboutUs);
        }
        [HttpPost]
        public IActionResult DeleteAboutUs(AboutUsViewModel vm)
        {

            if (ModelState.IsValid)
            {
                var AboutUs = _DbContext.AboutUs.Find(vm.Id);
                if (AboutUs == null)
                {
                    return NotFound();
                }

                _DbContext.AboutUs.Remove(AboutUs);
                _DbContext.SaveChangesAsync();
                return RedirectToAction("AboutUs");
            }

            return View(vm);

        }



    }
}
