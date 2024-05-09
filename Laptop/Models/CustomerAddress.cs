using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class CustomerAddress
    {
        public int CustomerId { get; set; }
        public int AddressNoteId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Name { get; set; }
        public bool? IsDefault { get; set; }
        public string Address { get; set; } = null!;

        public virtual AddressNotebook AddressNote { get; set; } = null!;
        public virtual Customer Customer { get; set; } = null!;
    }
}
