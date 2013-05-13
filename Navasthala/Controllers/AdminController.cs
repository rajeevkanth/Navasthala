using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DataLayer.Models;
using Navasthala.ViewModel;
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
            return View("UserDetailsView");
        }

        public ActionResult List(string sidx, string sord, int page, int rows)
        {


            var roles = (SimpleRoleProvider)Roles.Provider;
        
            var results = (from user in _context.UserProfiles.Include("Investments").AsEnumerable()
                               where user.IsActive && roles.GetRolesForUser(user.UserName).Contains("Investor")
                               select new{User=user,Investment=user.Investments});

            var investors = new List<InvestorViewModel>();

            foreach (var result in results)
            {
                if (!result.Investment.Any())
                {
                    var vm = new InvestorViewModel
                        {
                            UserName = result.User.UserName,
                            FirstName = result.User.FirstName,
                            LastName = result.User.LastName,
                            DateOfBirth = result.User.DateOfBirth,
                            Email =result.User.Email
                        };
                    investors.Add(vm);
                    continue;
                }

                investors.AddRange(result.Investment.Select(investment => new InvestorViewModel
                    {
                        UserName = result.User.UserName, FirstName = result.User.FirstName, LastName = result.User.LastName, DateOfBirth = result.User.DateOfBirth, Email = result.User.Email, DateOfInvestment = investment.DateOfInvestment, InvestedAmount = investment.InvestedAmount, Maturity = investment.Maturity, FinalAmount = investment.FinalAmount,InvestorId = investment.Id
                    }));
            }

            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = investors.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from inv in investors
                    select new
                    {
                        i = inv.UserName,
                        cell = new[] {
                            inv.UserName, 
                            inv.LastName, 
                            inv.Email,
                            inv.DateOfInvestment.HasValue ? inv.DateOfInvestment.Value.ToShortDateString() :null ,
                            inv.InvestedAmount.ToString(),
                            inv.Maturity.HasValue ? inv.Maturity.Value.ToShortDateString():null,
                            inv.FinalAmount.ToString(),
                            inv.InvestorId.ToString()

                        }
                    }).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(InvestorViewModel viewModel, FormCollection formCollection)
        {
            var operation = formCollection["oper"];
            if (operation.Equals("edit"))
            {
                var investment = _context.Investments.Include("UserProfile").FirstOrDefault(p => p.Id == viewModel.InvestorId);
                
                investment.UserProfile.LastName = viewModel.LastName;
                investment.UserProfile.Email = viewModel.Email;
                investment.DateOfInvestment = viewModel.DateOfInvestment;
                investment.InvestedAmount = viewModel.InvestedAmount;
                investment.Maturity = viewModel.Maturity;
                investment.FinalAmount = viewModel.FinalAmount;
                //_context.Entry(investment.UserProfile).State = EntityState.Modified;

                

            }
            else if (operation.Equals("del"))
            {

            }

            return null; //Content(repository.HasErrors.ToString().ToLower()); 

        }
    }
}
