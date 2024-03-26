using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class Ram
    {
        public Ram()
        {
            ProductVariations = new HashSet<ProductVariation>();
        }

        public int RamId { get; set; }
        public string? RamName { get; set; }

        public virtual ICollection<ProductVariation> ProductVariations { get; set; }
    }
}
