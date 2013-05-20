using System;
using System.ComponentModel.DataAnnotations;

namespace Navasthala.ViewModel
{
    public class ContactViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name, please")]
        public string Name { get; set; }

        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "We need you email address to contact you.")]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(1500)]
        public string Message { get; set; }

        public bool SubscribeForNewsLetter { get; set; }

        public DateTime? Date { get; set; }
    }
}