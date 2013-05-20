using System;
using System.Linq;
using System.Web.Mvc;
using DataLayer.Models;

namespace Navasthala.Controllers
{

    public class InvestorController : Controller
    {
        private readonly NavasthalaContext _context;
        
        public InvestorController(NavasthalaContext context)
        {
            _context = context;
        }

        [Authorize]
        public ActionResult Index()
        {

            if (User.IsInRole("Admin") || User.IsInRole("Investor"))
                return View();
            
            



            return RedirectToAction("UnAuthorised", "Home");
        }

        [Authorize]
        public ActionResult GetInvestments(string sidx, string sord, int page, int rows)
        {
            var investments =
                   _context.Investments.Include("UserProfile")
                           .Where(p => p.UserProfile.UserName.ToLower().Equals(User.Identity.Name.ToLower()))
                           .ToList();

            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = investments.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from inv in investments
                    select new
                    {
                        i = inv.Id,
                        cell = new[] {
                                inv.DateOfInvestment.HasValue ? inv.DateOfInvestment.Value.ToShortDateString() :null ,
                                inv.InvestedAmount.ToString(),
                                inv.Maturity.HasValue ? inv.Maturity.Value.ToShortDateString():null,
                                inv.FinalAmount.ToString(),
                                inv.Rate.ToString(),
                            }
                    }).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);



        }
    }
}
