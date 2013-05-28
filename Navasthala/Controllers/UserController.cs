using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using DataLayer.Models;
using Navasthala.ViewModel;
using WebMatrix.WebData;

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
                var roleProvider = (SimpleRoleProvider)Roles.Provider;
                var vm = new UserDetailsViewModel
                    {
                        DateOfBirth = user.DateOfBirth,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        Role = roleProvider.GetRolesForUser(user.UserName).FirstOrDefault(),
                        Addresses = GetUserAddresses(user.UserId),
                        Phones = GetUserPhones(user.UserId)
                    };
                return View("UserView", vm);
            }
            return View("UserView");
        }

        private List<PhoneViewModel> GetUserPhones(int userId)
        {
            return _context.Phones.Include("PhoneType").Where(p => p.UserId == userId).Select(p => new PhoneViewModel
                {
                    PhoneNumber = p.PhoneNumber,
                    PhoneType = p.PhoneType.Type
                }).ToList();
        }

        private List<AddressViewModel> GetUserAddresses(int userId)
        {
            return _context.Addresses.Include("AddressType").Where(p => p.UserId == userId && p.IsActive).Select(p=> new AddressViewModel
                {
                   AddressLineOne = p.AddressLineOne,
                   AddressLineTwo = p.AddressLineTwo,
                   Suburb = p.Suburb,
                   State = p.State,
                   City = p.City,
                   Country = p.Country,
                   AddressType = p.AddressType.Type
                }).ToList();
          
        }

        [HttpPost]
        public ActionResult PersonalDetails(UserDetailsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _context.UserProfiles.First(p => p.UserName.ToLower().Equals(viewModel.UserName.ToLower()));

                user.LastName = viewModel.LastName;
                user.FirstName = viewModel.FirstName;
                user.Email = viewModel.Email;
                user.DateOfBirth = viewModel.DateOfBirth;
                _context.SaveChanges();
                return RedirectToAction("Index", "User");
            }
            return View("UserPersonalDetailsView");
        }

        [Authorize]
        public ActionResult GetAddresses(string sidx, string sord, int page, int rows)
        {
            var user = _context.UserProfiles.FirstOrDefault(p => p.UserName.ToLower().Equals(User.Identity.Name));
            var addresses = _context.Addresses.Where(p => p.UserId == user.UserId && p.IsActive).AsEnumerable().Select(p => new AddressViewModel
                {
                    AddressLineOne = p.AddressLineOne,
                    AddressLineTwo = p.AddressLineTwo,
                    Suburb = p.Suburb,
                    City = p.City,
                    State = p.State,
                    Country = p.Country,
                    AddressType = _context.AddressTypes.Where(a=>a.Id==p.AddressType.Id).Select(a=>a.Type).FirstOrDefault(),
                    AddressId = p.Id,
                    
                }).ToList();

            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = addresses.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from adr in addresses
                    select new
                    {
                        i = adr.AddressId,
                        cell = new[] {
                                adr.AddressLineOne,
                                adr.AddressLineTwo,
                                adr.Suburb,
                                adr.City,
                                adr.State,
                                adr.Country,
                                adr.AddressType,
                                adr.AddressId.ToString()
                            }
                    }).ToArray()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        [Authorize]
        public ActionResult UpdateAddress(AddressViewModel viewModel, FormCollection formCollection)
        {
            var user = _context.UserProfiles.FirstOrDefault(p => p.UserName.ToLower().Equals(User.Identity.Name));
            var operation = formCollection["oper"];
            Address addr;
            switch (operation)
            {
                case "edit":
                    addr = _context.Addresses.FirstOrDefault(p => p.Id == viewModel.AddressId);
                    addr.AddressLineOne = viewModel.AddressLineOne;
                    addr.AddressLineTwo = viewModel.AddressLineTwo;
                    addr.City = viewModel.City;
                    addr.Country = viewModel.Country;
                    addr.Suburb = viewModel.Suburb;
                    addr.AddressType = _context.AddressTypes.FirstOrDefault(p => p.Type.Equals(viewModel.AddressType));

                    _context.SaveChanges();
                    break;

                case "add":


                    var model = new Address
                        {
                            AddressLineOne = viewModel.AddressLineOne,
                            AddressLineTwo = viewModel.AddressLineTwo,
                            Suburb = viewModel.Suburb,
                            City = viewModel.City,
                            State = viewModel.State,
                            Country = viewModel.Country,
                            IsActive = true,
                            Type = _context.AddressTypes.First(p => p.Type.TrimEnd().Equals(viewModel.AddressType)).Id,
                                 UserId = user.UserId
                        };
                    _context.Addresses.Add(model);

                    _context.SaveChanges();
                    break;

                case "del":
                    addr = _context.Addresses.FirstOrDefault(p => p.Id == viewModel.AddressId);
                    addr.IsActive = false;
                    _context.SaveChanges();
                    break;
            }

            return null;
        }

        [Authorize]
        public ActionResult Address()
        {
            return View("UserAddressView");
        }

        [Authorize]
        public ActionResult PersonalDetails()
        {
            UserDetailsViewModel vm = null;
            var user = _context.UserProfiles.FirstOrDefault(p => p.UserName.ToLower().Equals(User.Identity.Name));
            if (user != null)
            {

                vm = new UserDetailsViewModel
                    {
                        DateOfBirth = user.DateOfBirth,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName,

                    };
            }

           return View("UserPersonalDetailsView", vm);
            
             
        }

        [Authorize]
        public ActionResult PhoneDetails()
        {
            return View("UserPhoneView");
        }

        public ActionResult UpdatePhone(PhoneViewModel viewModel, FormCollection formCollection)
        {
            var user = _context.UserProfiles.FirstOrDefault(p => p.UserName.ToLower().Equals(User.Identity.Name));
            var operation = formCollection["oper"];

            Phone phone;
            switch (operation)
            {
                case "edit":
                    
                    phone = _context.Phones.Find(viewModel.PhoneId);
                    phone.PhoneNumber = viewModel.PhoneNumber;
                    phone.Type = _context.PhoneTypes.First(p => p.Type.ToLower().Equals(viewModel.PhoneType)).Id;
                    _context.SaveChanges();

                    break;

                case "add":
                    _context.Phones.Add(new Phone
                        {
                            PhoneNumber = viewModel.PhoneNumber,
                            Type =
                                 _context.PhoneTypes.First(p => p.Type.ToLower().Equals(viewModel.PhoneType)).Id,
                                 IsActive = true,
                                 UserId = user.UserId
                        });
                    
                        _context.SaveChanges();

                    break;

                case "del":
                    phone = _context.Phones.Find(viewModel.PhoneId);
                    phone.IsActive = false;
                    _context.SaveChanges();
                    break;
            }
            return null;

        }

        public ActionResult GetUserPhones(string sidx, string sord, int page, int rows)
        {
            var user = _context.UserProfiles.FirstOrDefault(p => p.UserName.ToLower().Equals(User.Identity.Name));
            var phones =
                _context.Phones.Include("PhoneType")
                        .AsEnumerable()
                        .Where(p => p.UserId == user.UserId && p.IsActive)
                        .Select(p => new PhoneViewModel
                            {
                                PhoneId = p.Id,
                                PhoneNumber = p.PhoneNumber,
                                PhoneType = p.PhoneType.Type
                            });

            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = phones.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from phone in phones
                    select new
                    {
                        i = phone.PhoneId,
                        cell = new[] {
                             phone.PhoneNumber,
                             phone.PhoneType,
                             phone.PhoneId.ToString()
                            }
                    }).ToArray()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }
    }
}
