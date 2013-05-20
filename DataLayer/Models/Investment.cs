using System;

namespace DataLayer.Models
{
    public partial class Investment
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateOfInvestment { get; set; }
        public double? InvestedAmount { get; set; }
        public double? Rate { get; set; }
        public DateTime? Maturity { get; set; }
        public double? FinalAmount { get; set; }
        public int UserId { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}
