using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class ProductItem
    {
        public ProductItem()
        {
            ProductVariations = new HashSet<ProductVariation>();
        }

        public int ProductItemsId { get; set; }
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public string? ProductCode { get; set; }
        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
        public string? Image4 { get; set; }

        public virtual Color Color { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
        public virtual ICollection<ProductVariation> ProductVariations { get; set; }
    }
}
