using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductItems = new HashSet<ProductItem>();
        }

        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int? CategoryId { get; set; }
        public string? Description { get; set; }
        public int? Brand { get; set; }
        

        public bool New { get; set; }

        public bool Hot { get; set; }

        public bool TopSelling { get; set; }


        public virtual Brand? BrandNavigation { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<ProductItem> ProductItems { get; set; }
    }
}
