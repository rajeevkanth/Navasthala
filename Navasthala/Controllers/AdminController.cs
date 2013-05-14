﻿using System;
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

        private UserViewModel _vm;

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
            _vm = vm;
            return View("ManageUsersView",vm);
        }

        [HttpPost]
        public ActionResult ManageUsers(UserViewModel vm)
        {
            SetRoles();
            _vm = vm;
            return View("ManageUsersView");
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

        public ActionResult ListInvestors(string sidx, string sord, int page, int rows)
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
                            Email =result.User.Email,
                            UserId = result.User.UserId
                        };
                    investors.Add(vm);
                    continue;
                }

                investors.AddRange(result.Investment.Select(investment => new InvestorViewModel
                    {
                        UserName = result.User.UserName,
                        FirstName = result.User.FirstName,
                        LastName = result.User.LastName,
                        DateOfBirth = result.User.DateOfBirth,
                        Email = result.User.Email,
                        DateOfInvestment = investment.DateOfInvestment,
                        InvestedAmount = investment.InvestedAmount,
                        Maturity = investment.Maturity,
                        FinalAmount = investment.FinalAmount,
                        InvestmentId = investment.Id,
                        UserId = result.User.UserId
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
                            inv.LastName, 
                            inv.Email,
                            inv.DateOfInvestment.HasValue ? inv.DateOfInvestment.Value.ToShortDateString() :null ,
                            inv.InvestedAmount.ToString(),
                            inv.Maturity.HasValue ? inv.Maturity.Value.ToShortDateString():null,
                            inv.FinalAmount.ToString(),
                            inv.InvestmentId.ToString(),
                            inv.UserId.ToString()

                        }
                    }).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateInvestor(InvestorViewModel viewModel, FormCollection formCollection)
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
                }
                else
                {
                    var user = _context.UserProfiles.FirstOrDefault(p => p.UserId == viewModel.UserId);
                    user.LastName = viewModel.LastName;
                    user.Email = viewModel.Email;


                    _context.Investments.Add(new Investment
                        {
                            DateOfInvestment = viewModel.DateOfInvestment,
                            InvestedAmount = viewModel.InvestedAmount,
                            Maturity = viewModel.Maturity,
                            FinalAmount = viewModel.FinalAmount,
                            UserProfile = user
                        });
                }
                _context.SaveChanges();



            }
            else if (operation.Equals("del"))
            {

            }

            return null; //Content(repository.HasErrors.ToString().ToLower()); 

        }

        public ActionResult UpdateUser(UserViewModel viewModel, FormCollection formCollection)
        {
            return null;
        }

        public ActionResult GetUsers(string sidx, string sord, int page, int rows)
        {
            
            var roleProvider = (SimpleRoleProvider)Roles.Provider;
            var users = _context.UserProfiles.Where(p=>p.UserName != User.Identity.Name).AsEnumerable();

            //if (string.IsNullOrEmpty(_vm.UserName))
            //{
            //    users = users.Where(p => p.UserName.Contains(_vm.UserName));
            //}

            //if (string.IsNullOrEmpty(_vm.LastName))
            //{
            //    users = users.Where(p => p.LastName.Contains(_vm.LastName));
            //}

            //if (string.IsNullOrEmpty(_vm.FirstName))
            //{
            //    users = users.Where(p => p.FirstName.Contains(_vm.FirstName));
            //}
            
            //if (_vm.DateOfBirth.HasValue)
            //{
            //    users = users.Where(p => p.DateOfBirth.HasValue && p.DateOfBirth.Value.ToShortDateString().Equals(_vm.DateOfBirth.Value.ToShortDateString()));
            //}
            
            //if (string.IsNullOrEmpty(_vm.Email))
            //{
            //    users = users.Where(p => p.Email.Contains(_vm.Email));
            //}

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
                            usr.DateOfBirth,
                            usr.Role,
                            usr.UserId.ToString()

                        }
                    }).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
      
        }

       

    }
}
