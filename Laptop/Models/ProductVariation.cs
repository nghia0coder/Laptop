using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class ProductVariation
    {
        public ProductVariation()
        {
            InvoiceDetails = new HashSet<InvoiceDetail>();
            OrdersDetails = new HashSet<OrdersDetail>();
        }

        public int ProductVarId { get; set; }
        public int ProductItemsId { get; set; }
        public int RamId { get; set; }
        public int Ssdid { get; set; }
        public int QtyinStock { get; set; }
        public int? OriginPrice { get; set; }
        public int Price { get; set; }

        public int? WarrantyTime { get; set; }

        public virtual ProductItem ProductItems { get; set; } = null!;
        public virtual Ram Ram { get; set; } = null!;
        public virtual Ssd Ssd { get; set; } = null!;

        public virtual ICollection<WishList> WishLists { get; set; }
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
        public virtual ICollection<OrdersDetail> OrdersDetails { get; set; }
    }
}
