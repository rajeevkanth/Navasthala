using System;
using System.ComponentModel.DataAnnotations;

namespace Navasthala.ViewModel
{
    public class UserDetailsViewModel
    {
        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

       public string UserName { get; set; }
    }
}