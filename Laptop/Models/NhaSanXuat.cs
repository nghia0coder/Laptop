using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class NhaSanXuat
    {
        public NhaSanXuat()
        {
            NhaCungCaps = new HashSet<NhaCungCap>();
        }

        public int Idnhasx { get; set; }
        public string? Tennhasx { get; set; }
        public string? Diachi { get; set; }
        public string? Sđt { get; set; }
        public string? Email { get; set; }

        public virtual ICollection<NhaCungCap> NhaCungCaps { get; set; }
    }
}
