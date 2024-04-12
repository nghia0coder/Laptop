using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class Order
    {
        public Order()
        {
            OrdersDetails = new HashSet<OrdersDetail>();
        }

        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public bool Delivered { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string CustomerId { get; set; } = null!;
        public bool Status { get; set; }
        public string? Voucher { get; set; }

        public int ? Total { get; set; }

        public virtual AppUserModel Customer { get; set; } = null!;
        public virtual Voucher? VoucherNavigation { get; set; }
        public virtual ICollection<OrdersDetail> OrdersDetails { get; set; }
    }
}
