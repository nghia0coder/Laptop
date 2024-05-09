using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class AddressNotebook
    {
        public AddressNotebook()
        {
            CustomerAddresses = new HashSet<CustomerAddress>();
        }

        public int AddressNoteId { get; set; }
        public int? AddressId { get; set; }

        public virtual Ward? Address { get; set; }
        public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; }
    }
}
