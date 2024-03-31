using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Laptop.Models
{
    public partial class Color
    {
        public Color()
        {
            //uhuhuhu
            ProductItems = new HashSet<ProductItem>();
        }

        public int ColorId { get; set; }
        [DisplayName("Tên Màu")]
        public string? ColorName { get; set; }

        public virtual ICollection<ProductItem> ProductItems { get; set; }
    }
}
