using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataLayer.CustomValidator;

namespace Navasthala.ViewModel
{
    public class UserDetailsViewModel
    {
        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }
         
        [DataType(DataType.EmailAddress)]
        [Required]
        [UniqueEmailValidator(ErrorMessage = "This email address cant be used.")]
        public string Email { get; set; }


        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        public string Role { get; set; }

        public string UserName { get; set; }

        public List<AddressViewModel> Addresses { get; set; }

        public List<PhoneViewModel> Phones { get; set; }
    }
}