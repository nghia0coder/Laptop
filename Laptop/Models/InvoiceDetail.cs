using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class InvoiceDetail
    {
        public int ProductVarId { get; set; }
        public int InvoiceId { get; set; }
        public int? Quanity { get; set; }
        public int? Price { get; set; }

        public virtual Invoice Invoice { get; set; } = null!;
        public virtual ProductVariation ProductVar { get; set; } = null!;
    }
}
