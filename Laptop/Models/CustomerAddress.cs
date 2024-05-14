using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class CustomerAddress
    {
        public int CustomerAddressId { get; set; }
        public int? CustomerId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Name { get; set; }
        public int? City { get; set; }
        public int? Ward { get; set; }
        public int? District { get; set; }
        public string AddressLine { get; set; } = null!;

        public virtual Customer? Customer { get; set; }
    }
}
