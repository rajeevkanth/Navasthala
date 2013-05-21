using System.Linq;
using System.Web.Mvc;
using DataLayer.Models;
using Navasthala.ViewModel;

namespace Navasthala.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly NavasthalaContext _context;
        
        public UserController(NavasthalaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Index()
        {
           var user = _context.UserProfiles.FirstOrDefault(p => p.UserName.ToLower().Equals(User.Identity.Name));
            if (user != null)
            {
                //ViewBag.LastName = (string) user.LastName;
                //ViewBag.FirstName = user.FirstName;
                //ViewBag.Email = user.Email;
                //ViewBag.DateOfBirth = user.DateOfBirth.HasValue ? user.DateOfBirth.Value.ToShortDateString(): string.Empty;
                var vm = new UserDetailsViewModel
                    {
                        DateOfBirth = user.DateOfBirth,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName
                    };
                return View("UserView", vm);
            }
            return View("UserView");
        }

        [HttpPost]
        public ActionResult Index(UserDetailsViewModel viewModel)
        {
            //Because of client side validation, model state is always valid
            //So save and return null
            //The page will get reloaded.
            var user = _context.UserProfiles.First(p => p.UserName.ToLower().Equals(viewModel.UserName.ToLower()));

            user.LastName = viewModel.LastName;
            user.FirstName = viewModel.FirstName;
            user.Email = viewModel.Email;
            user.DateOfBirth = viewModel.DateOfBirth;
            _context.SaveChanges();

            return new EmptyResult();
        }
    }
}
