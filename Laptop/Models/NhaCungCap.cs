using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "Tên nhà cung cấp không được để trống.")]
        public string Tennhacc { get; set; } = null!;

        [Required(ErrorMessage = "Địa chỉ không được để trống.")]
        public string Diachi { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        public string Sdt { get; set; }

        [Required(ErrorMessage = "Email không được để trống.")]
        public string Email { get; set; }
        public int Idnhasx { get; set; }

        public virtual NhaSanXuat IdnhasxNavigation { get; set; } = null!;
        public virtual ICollection<PhieuNhap> PhieuNhaps { get; set; }
        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
