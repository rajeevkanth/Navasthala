using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Navasthala.ViewModel
{
    public class AddInvestorViewModel
    {
        [Required]
        public string InvestorName { get; set; }

        public DateTime? DateOfInvestment { get; set; }

        public double? InvestedAmount { get; set; }

        public DateTime? Maturity { get; set; }

        public double? FinalAmount { get; set; }

        public double? Rate { get; set; }
    }
}