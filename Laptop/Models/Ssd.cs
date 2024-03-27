using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class Ssd
    {
        public Ssd()
        {
            ProductVariations = new HashSet<ProductVariation>();
        }

        public int SsdId { get; set; }
        public string? Ssdname { get; set; }

        public virtual ICollection<ProductVariation> ProductVariations { get; set; }
    }
}
