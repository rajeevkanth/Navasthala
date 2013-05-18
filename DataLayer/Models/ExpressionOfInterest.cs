using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
    public partial class ExpressionOfInterest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime? Date { get; set; }
        public bool? SubscribeForNewsLetter { get; set; }
    }
}
