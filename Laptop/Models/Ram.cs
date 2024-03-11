using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class Ram
    {
        public Ram()
        {
            SanPhams = new HashSet<SanPham>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
