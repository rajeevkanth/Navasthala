using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Navasthala.ViewModel
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            Search = new SearchModel();
        }

        public SearchModel Search { get; set; }
        public IList<string> Roles { get; set; }

        public string FirstName { get; set; }

        public string UserName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Role { get; set; }

        public int UserId { get; set; }
    }

    public class SearchModel
    {

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Date of birth")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Role")]
        public int RoleId { get; set; }

    }

    public class Role
    {
        public int Id { get; set; }

        public string Name { get; set; }

    }
}