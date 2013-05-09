using System.Web.Mvc;

namespace Navasthala.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult UnAuthorised()
        {
            return View("UnAuthorisedView");
        }

        public ActionResult Contact()
        {
            return View();
        }

        [Authorize]
        public ActionResult User()
        {
            if (base.User.IsInRole("Admin"))
            {
                return View("AdminView");
            }
            
            if(base.User.IsInRole("Investor"))
                return RedirectToAction("Index", "Investor");

            return View("UserView");
        }
    }
}
