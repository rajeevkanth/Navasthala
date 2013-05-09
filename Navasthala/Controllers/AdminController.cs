using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DataLayer.Models;
using WebMatrix.WebData;

namespace Navasthala.Controllers
{
    public class AdminController : Controller
    {
        private readonly NavasthalaContext _context;

        public AdminController(NavasthalaContext context)
        {
            _context = context;
        }
        //
        // GET: /Admin/
        [Authorize]
        public ActionResult DashBoard()
        {
            return View();
        }

        [Authorize]
        public ActionResult Investors()
        {
            var roles = (SimpleRoleProvider)Roles.Provider;

            var investors = (from user in _context.UserProfiles.AsEnumerable()
                            where roles.GetRolesForUser(user.UserName).Contains("Investor")
                            select user).ToList();

            return View("UserDetailsView",investors);
        }       

    }
}
