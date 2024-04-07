using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Laptop.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductItems = new HashSet<ProductItem>();
            ProductComments = new HashSet<ProductComment>(); 
        }

        public int ProductId { get; set; }
        [DisplayName("Tên Sản Phẩm")]
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

        public virtual ICollection<ProductComment> ProductComments { get; set; }

    }
}
