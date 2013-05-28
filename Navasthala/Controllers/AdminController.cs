using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult ManageUsers()
        {
             SetRoles();  
             var vm = new UserViewModel();
            return View("ManageUsersView",vm);
        }

        private void SetRoles()
        {
            var roleProvider = (SimpleRoleProvider)Roles.Provider;
            var roles = roleProvider.GetAllRoles().ToList();
            roles.Insert(0, "All");
            ViewBag.Roles = new SelectList(roles);
        }

        [Authorize]
        public ActionResult Investors()
        {
            return View("InvestorDetailsView");
        }

        public ActionResult ExpressionOfInterest()
        {
            return View("EOIView");
        }

        public ActionResult GetInvestments(string sidx, string sord, int page, int rows)
        {
            var investments = _context.Investments.Include("UserProfile").Where(p => p.IsActive).ToArray();

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
                                inv.UserProfile.UserName, 
                                inv.UserProfile.Email,
                                inv.DateOfInvestment.HasValue ? inv.DateOfInvestment.Value.ToShortDateString() :null ,
                                inv.InvestedAmount.ToString(),
                                inv.Maturity.HasValue ? inv.Maturity.Value.ToShortDateString():null,
                                inv.FinalAmount.ToString(),
                                inv.Rate.ToString(),
                                inv.Id.ToString()

                            }
                    }).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        
        public ActionResult GetInvestors()
        {
            var roleProvider = (SimpleRoleProvider)Roles.Provider;
            var results = (from user in _context.UserProfiles.Where(p => p.IsActive).AsEnumerable()
                           where roleProvider.GetRolesForUser(user.UserName).Contains("Investor")
                            select new
                               {
                                   user.UserName
                                   
                               }).ToArray();

             return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateInvestment(InvestorViewModel viewModel, FormCollection formCollection)
        {
            var operation = formCollection["oper"];
            if (operation.Equals("edit"))
            {
                var investment = _context.Investments.Include("UserProfile").FirstOrDefault(p => p.Id == viewModel.InvestmentId);
                if (investment != null)
                {
                    investment.UserProfile.LastName = viewModel.LastName;
                    investment.UserProfile.Email = viewModel.Email;
                    investment.DateOfInvestment = viewModel.DateOfInvestment;
                    investment.InvestedAmount = viewModel.InvestedAmount;
                    investment.Maturity = viewModel.Maturity;
                    investment.FinalAmount = viewModel.FinalAmount;
                    investment.Rate = viewModel.Rate;
                }
                _context.SaveChanges();
            }
            else if (operation.Equals("del"))
            {
                var investment = _context.Investments.Include("UserProfile").FirstOrDefault(p => p.Id == viewModel.InvestmentId);
                if (investment != null)
                {
                    investment.IsActive = false;
                }
                _context.SaveChanges();
            }

            return null; //Content(repository.HasErrors.ToString().ToLower()); 

        }
        [HttpPost]
        public ActionResult AddInvestment(AddInvestorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                var user =
                    _context.UserProfiles.FirstOrDefault(
                        p => p.UserName.ToLower().Equals(viewModel.InvestorName.ToLower()));

                _context.Investments.Add(new Investment
                {
                    DateOfInvestment = viewModel.DateOfInvestment,
                    InvestedAmount = viewModel.InvestedAmount,
                    Maturity = viewModel.Maturity,
                    FinalAmount = viewModel.FinalAmount,
                    IsActive = true,
                    UserProfile = user,
                    Rate = viewModel.Rate
                });
                _context.SaveChanges();
                return Json("Success");
            }
            return Json("Invalid Parameters");
        }

        public ActionResult UpdateUser(UserViewModel viewModel, FormCollection formCollection)
        {
            var operation = formCollection["oper"];
            if (operation.Equals("edit"))
            {
                var user = _context.UserProfiles.FirstOrDefault(p => p.UserId == viewModel.UserId);
                if (user != null)
                {
                    var roleProvider = (SimpleRoleProvider)Roles.Provider;
                    
                    roleProvider.RemoveUsersFromRoles(new[]{user.UserName},roleProvider.GetRolesForUser(user.UserName));
                    roleProvider.AddUsersToRoles(new[] { user.UserName }, new[] { viewModel.Role.TrimStart() });
                    _context.SaveChanges();
                  

                    user.FirstName = viewModel.FirstName;
                    user.LastName = viewModel.LastName;
                    user.Email = viewModel.Email;
                    user.DateOfBirth = viewModel.DateOfBirth;

                    _context.SaveChanges();
                }
            }

            if (operation.Equals("del"))
            {
                var user = _context.UserProfiles.FirstOrDefault(p => p.UserId == viewModel.UserId);
                user.IsActive = false;
                _context.SaveChanges();
            }


            return null;
        }

        public ActionResult GetUsers(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            var roleProvider = (SimpleRoleProvider)Roles.Provider;
            var users = _context.UserProfiles.Where(p=>p.UserName != User.Identity.Name && p.IsActive).AsEnumerable();

            if (_search)
            {
                switch (searchField)
                {
                    case "UserName":
                        users = users.Where(p => p.UserName.ToLower().Contains(searchString.ToLower()));
                        break;

                    case "LastName":
                        users = users.Where(p => p.LastName!= null && p.LastName.ToLower().Contains(searchString.ToLower()));
                        break;

                    case "FirstName":
                        users = users.Where(p => p.FirstName!= null && p.FirstName.ToLower().Contains(searchString.ToLower()));
                        break;

                    case "Email":
                        users = users.Where(p => p.Email!= null && p.Email.ToLower().Contains(searchString.ToLower()));
                        break;

                    default:
                        break;
                }
            }

           var filteredUsers= users.Select(p => new UserViewModel
                {
                    FirstName = p.UserName,
                    LastName = p.LastName,
                    UserName = p.UserName,
                    Email = p.Email,
                    DateOfBirth = p.DateOfBirth,
                    UserId = p.UserId,
                    Role = roleProvider.GetRolesForUser(p.UserName).FirstOrDefault()
                });

           var pageIndex = Convert.ToInt32(page) - 1;
           var pageSize = rows;
           var totalRecords = users.Count();
           var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);


            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from usr in filteredUsers
                    select new
                    {
                        i = usr.UserName,
                        cell = new object[] {
                            usr.UserName, 
                            usr.LastName,
                            usr.FirstName,
                            usr.Email,
                            usr.DateOfBirth.HasValue? usr.DateOfBirth.Value.ToShortDateString() :string.Empty,
                            usr.Role,
                            usr.UserId.ToString()

                        }
                    }).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
      
        }

        public ActionResult GetEoi(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            var eoi = _context.ExpressionOfInterests.AsEnumerable();
            if (_search)
            {
                switch (searchField)
                {
                    case "Name":
                        eoi = eoi.Where(p => p.Name != null && p.Name.ToLower().Equals(searchString.ToLower()));
                        break;

                    case "Phone":
                        eoi = eoi.Where(p => p.Phone != null && p.Phone.ToLower().Equals(searchString.ToLower()));
                        break;

                    case "Email":
                        eoi = eoi.Where(p => p.Email != null && p.Email.ToLower().Equals(searchString.ToLower()));
                        break;

                    case "Date":
                        eoi = eoi.Where(p => p.Date != null && p.Date.Value.ToShortDateString().Equals(searchString.ToLower()));
                        break;
                   
                }
            }

            var filteredEoi = eoi.Select(p => new ContactViewModel
            {
                Name = p.Name,
                Phone = p.Phone,
                Email = p.Email,
                Message = p.Message,
                Date = p.Date,
                Id=p.Id
            });

            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = eoi.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);


            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from usr in filteredEoi
                    select new
                    {
                        i = usr.Id,
                        cell = new object[] {
                            usr.Name, 
                            usr.Email,
                            usr.Phone,
                            usr.Date.HasValue? usr.Date.Value.ToShortDateString():string.Empty,
                            usr.Message
                        }
                    }).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }
   

    }
}
