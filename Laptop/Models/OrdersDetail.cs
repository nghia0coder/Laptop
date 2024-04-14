using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class OrdersDetail
    {
        public string OrderId { get; set; }
        public int ProductVarId { get; set; }
        public int? Quanity { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual ProductVariation ProductVar { get; set; } = null!;
    }
}
