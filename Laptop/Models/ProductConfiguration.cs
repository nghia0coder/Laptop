using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class ProductConfiguration
    {
        public ProductConfiguration()
        {
            SanPhams = new HashSet<SanPham>();
        }

        public int ConfigId { get; set; }
        public string? Config1 { get; set; }
        public string? Config2 { get; set; }
        public string? Config3 { get; set; }
        public string? Config4 { get; set; }
        public string? Config5 { get; set; }


        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
