using System;

namespace Navasthala.ViewModel
{
    public class InvestorViewModel
    {
        public int InvestmentId { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Email { get; set; }

        public DateTime? DateOfInvestment { get; set; }

        public double? InvestedAmount { get; set; }

        public DateTime? Maturity { get; set; }

        public double? FinalAmount { get; set; }


    }
}