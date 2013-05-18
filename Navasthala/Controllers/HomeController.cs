using System;
using System.Web.Mvc;
using DataLayer.Models;
using Navasthala.ViewModel;

namespace Navasthala.Controllers
{
    public class HomeController : Controller
    {
        private readonly NavasthalaContext _context;

        public HomeController(NavasthalaContext context)
        {
            _context = context;
        }

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
            var model = new ContactViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.ExpressionOfInterests.Add(new ExpressionOfInterest
                    {
                        Date = DateTime.UtcNow,
                        Name = model.Name,
                        Phone = model.Phone,
                        Email = model.Email,
                        Message = model.Message,
                        SubscribeForNewsLetter = model.SubscribeForNewsLetter
                    });

                _context.SaveChanges();
                return View("ContactConfirmation");
            }
            return View();
        }

        [Authorize]
        public ActionResult CurrentUser()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("DashBoard","Admin");
            }
            
            if(User.IsInRole("Investor"))
                return RedirectToAction("Index", "Investor");

            return View("UserView");
        }
    }
}
