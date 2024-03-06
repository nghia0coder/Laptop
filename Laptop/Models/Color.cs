using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class Color
    {
        public Color()
        {
            SanPhams = new HashSet<SanPham>();
        }

        public int ColorID { get; set; }
        public string? colorName { get; set; }
        public string? colorHex { get; set; }


        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
