namespace DataLayer.Models
{
    public partial class Phone
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public int Type { get; set; }
        public int UserId { get; set; }
        public virtual PhoneType PhoneType { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}
