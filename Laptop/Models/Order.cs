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

        public string OrderId { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public long? PriceTotal { get; set; }
        public int OrderStatus { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int? EmployeeId { get; set; }
        public int? CustomerId { get; set; }
        public bool StatusPayment { get; set; }
        public string? Voucher { get; set; }
        public string? DeliveryAddress { get; set; }
        public virtual Customer Customer { get; set; } = null!;
        public virtual Employee? Employee { get; set; }
        public virtual OrderStatus StatusNaviagtion { get; set; } = null!;
        public virtual Voucher? VoucherNavigation { get; set; }
        public virtual ICollection<OrdersDetail> OrdersDetails { get; set; }
    }
}
