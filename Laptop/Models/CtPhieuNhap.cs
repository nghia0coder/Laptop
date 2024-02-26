using System;
using System.Collections.Generic;

namespace GiayDep.Models
{
    public partial class CtPhieuNhap
    {
        public int IdchitietPn { get; set; }
        public int Idsp { get; set; }
        public int Idphieunhap { get; set; }
        public int? Soluong { get; set; }
        public decimal? Gia { get; set; }
        public DateTime NgayNhap { get; set; }

        public virtual PhieuNhap IdphieunhapNavigation { get; set; } = null!;
        public virtual SanPham IdspNavigation { get; set; } = null!;
    }
}
