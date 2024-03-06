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

        public int IdColor { get; set; }
        public string? ColorName { get; set; }
        public string? ColorHex { get; set; }

        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
