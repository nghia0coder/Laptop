using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class CtHoaDon
    {
        public int IdchitietDdh { get; set; }
        public int Idhoadon { get; set; }
        public int Idsp { get; set; }
        public string? Tensp { get; set; }
        public int? Soluong { get; set; }
        public decimal? Dongia { get; set; }

        public virtual HoaDon IdhoadonNavigation { get; set; } = null!;
        public virtual SanPham IdspNavigation { get; set; } = null!;
    }
}
