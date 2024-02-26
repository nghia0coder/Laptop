using System;
using System.Collections.Generic;

namespace GiayDep.Models
{
    public partial class PhieuNhap
    {
        public PhieuNhap()
        {
            CtPhieuNhaps = new HashSet<CtPhieuNhap>();
        }

        public int Idphieunhap { get; set; }
        public DateTime Ngaynhap { get; set; }
        public int Idnhacc { get; set; }

        public virtual NhaCungCap IdnhaccNavigation { get; set; } = null!;
        public virtual ICollection<CtPhieuNhap> CtPhieuNhaps { get; set; }
    }
}
