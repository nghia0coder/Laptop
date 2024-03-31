using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class Brand
    {
        public Brand()
        {
            News = new HashSet<News>();
            Products = new HashSet<Product>();
        }

        public int BrandId { get; set; }
        public string? BrandName { get; set; }

        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
