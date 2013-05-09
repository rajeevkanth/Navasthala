using System.Collections.Generic;

namespace DataLayer.Models
{
    public partial class PhoneType
    {
        public PhoneType()
        {
            Phones = new List<Phone>();
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public virtual ICollection<Phone> Phones { get; set; }
    }
}
