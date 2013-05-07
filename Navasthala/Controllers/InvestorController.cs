using System.Web.Mvc;

namespace Navasthala.Controllers
{

    public class InvestorController : Controller
    {

        [Authorize]
        public ActionResult Index()
        {

            if (User.IsInRole("Admin") || User.IsInRole("Investor"))
                return View("InvestorMainView");


            return RedirectToAction("UnAuthorised", "Home");
        }

    }
}
