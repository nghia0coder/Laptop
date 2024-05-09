using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class Ward
    {
        public Ward()
        {
            AddressNotebooks = new HashSet<AddressNotebook>();
        }

        public int WardsId { get; set; }
        public int DistrictId { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<AddressNotebook> AddressNotebooks { get; set; }
    }
}
