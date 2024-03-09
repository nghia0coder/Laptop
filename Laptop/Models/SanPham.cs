﻿using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class SanPham
    {
        public SanPham()
        {
            CtHoaDons = new HashSet<CtHoaDon>();
            CtPhieuNhaps = new HashSet<CtPhieuNhap>();
        }

        public int Idsp { get; set; }
        public string Tensp { get; set; } = null!;
        public int? Dongia { get; set; }
        public int? Soluong { get; set; }
        public string? Baohanh { get; set; }
        public string? Khuyenmai { get; set; }
        public int Maloaisp { get; set; }
        public int? Manhacc { get; set; }
        public string? Hinhanh1 { get; set; }
        public string? Description { get; set; }
        public string? Hinhanh2 { get; set; }
        public string? Hinhanh3 { get; set; }
        public string? Hinhanh4 { get; set; }
        public int? ColorId { get; set; }
        public int? Manhasx { get; set; }
        public int? Ram { get; set; }
        public int? Ssd { get; set; }

        public virtual Color? Color { get; set; }
        public virtual LoaiSp MaloaispNavigation { get; set; } = null!;
        public virtual NhaCungCap? ManhaccNavigation { get; set; }
        public virtual NhaSanXuat? ManhasxNavigation { get; set; }
        public virtual Ram? RamNavigation { get; set; }
        public virtual Ssd? SsdNavigation { get; set; }
        public virtual ICollection<CtHoaDon> CtHoaDons { get; set; }
        public virtual ICollection<CtPhieuNhap> CtPhieuNhaps { get; set; }
    }
}
