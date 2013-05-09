namespace DataLayer.Models
{
    public partial class Address
    {
        public int Id { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int Type { get; set; }
        public int UserId { get; set; }
        public virtual AddressType AddressType { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}
