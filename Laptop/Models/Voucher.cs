using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class Voucher
    {
        public Voucher()
        {
            Orders = new HashSet<Order>();
        }

        public string VoucherCode { get; set; } = null!;
        public int? Discount { get; set; }
        public int? VoucherQuantity { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
