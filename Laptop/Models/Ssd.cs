using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class Ssd
    {
        public Ssd()
        {
            SanPhams = new HashSet<SanPham>();
        }

        public int Id { get; set; }
        public string? DungLuong { get; set; }

        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
