using System;
using System.Collections.Generic;

namespace GiayDep.Models
{
    public partial class NhaCungCap
    {
        public NhaCungCap()
        {
            PhieuNhaps = new HashSet<PhieuNhap>();
            SanPhams = new HashSet<SanPham>();
        }

        public int Idnhacc { get; set; }
        public string Tennhacc { get; set; } = null!;
        public string? Diachi { get; set; }
        public string? Sdt { get; set; }
        public string? Email { get; set; }
        public int Idnhasx { get; set; }

        public virtual NhaSanXuat IdnhasxNavigation { get; set; } = null!;
        public virtual ICollection<PhieuNhap> PhieuNhaps { get; set; }
        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
