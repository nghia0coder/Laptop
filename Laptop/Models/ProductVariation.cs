using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class ProductVariation
    {
        public ProductVariation()
        {
            OrdersDetails = new HashSet<OrdersDetail>();
        }

        public int ProductVarId { get; set; }
        public int ProductItemsId { get; set; }
        public int RamId { get; set; }
        public int Ssdid { get; set; }
        public int? QtyinStock { get; set; }
        public int? Price { get; set; }

        public virtual ProductItem ProductItems { get; set; } = null!;
        public virtual Ram Ram { get; set; } = null!;
        public virtual Ssd Ssd { get; set; } = null!;
        public virtual InvoiceDetail? InvoiceDetail { get; set; }
        public virtual ICollection<OrdersDetail> OrdersDetails { get; set; }
    }
}
