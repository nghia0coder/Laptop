using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
