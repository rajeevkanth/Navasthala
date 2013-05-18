using System;
using System.ComponentModel.DataAnnotations;

namespace Navasthala.ViewModel
{
    public class ContactViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(1500)]
        public string Message { get; set; }

        public bool SubscribeForNewsLetter { get; set; }

        public DateTime? Date { get; set; }
    }
}