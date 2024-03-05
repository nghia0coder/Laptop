using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiayDep.Models
{
    public partial class SanPham
    {
        public SanPham()
        {
            CtHoaDons = new HashSet<CtHoaDon>();
            CtPhieuNhaps = new HashSet<CtPhieuNhap>();
        }

        public int Idsp { get; set; }

        [Required(ErrorMessage = "Tên nhà sản phẩm không được để trống.")]
        public string Tensp { get; set; }

        [Required(ErrorMessage = "Đơn giá không được để trống.")]
        public int Dongia { get; set; }

        [Required(ErrorMessage = "Số lượng không được để trống.")]
        public int? Soluong { get; set; }
        public string? Baohanh { get; set; }
        public string? Khuyenmai { get; set; }
        public int Maloaisp { get; set; }
        public int? Manhacc { get; set; }
        public string? Hinhanh1 { get; set; }
        public string? Hinhanh2 { get; set; }
        public string? Hinhanh3 {get; set; }
        public string? Hinhanh4 { get; set; }
        public string? Description { get; set; }

        public virtual LoaiSp MaloaispNavigation { get; set; } = null!;
        public virtual NhaCungCap? ManhaccNavigation { get; set; }
        public virtual ICollection<CtHoaDon> CtHoaDons { get; set; }
        public virtual ICollection<CtPhieuNhap> CtPhieuNhaps { get; set; }

        [NotMapped]
        public IFormFile Image1 { get; set; }
        [NotMapped]
        public IFormFile Image2 { get; set; }
        [NotMapped]
        public IFormFile Image3 { get; set; }
        [NotMapped]
        public IFormFile Image4 { get; set; }


    }
}
