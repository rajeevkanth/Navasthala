using System.Web.Mvc;

namespace Navasthala.Controllers
{

    public class InvestorController : Controller
    {

        [Authorize(Roles = "Investor" )]
        public ActionResult Index()
        {
            return View("InvestorMainView");
        }

    }
}
