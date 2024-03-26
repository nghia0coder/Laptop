using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class Color
    {
        public Color()
        {
            ProductItems = new HashSet<ProductItem>();
        }

        public int ColorId { get; set; }
        public string? ColorName { get; set; }

        public virtual ICollection<ProductItem> ProductItems { get; set; }
    }
}
