using System.Collections.Generic;

namespace DataLayer.Models
{
    public partial class AddressType
    {
        public AddressType()
        {
            this.Addresses = new List<Address>();
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
    }
}
