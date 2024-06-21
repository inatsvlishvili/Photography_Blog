using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Photography_Blog.Data;

namespace Photography_Blog.Controllers
{
    public class BaseController : Controller
    {
        public BlogContext _DbContext;
        
        public BaseController(BlogContext DbContext)
        {
            _DbContext = DbContext;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.ContactInfo = _DbContext.ContactInfos.FirstOrDefault();
            
            base.OnActionExecuting(filterContext);
        }
    }
}
