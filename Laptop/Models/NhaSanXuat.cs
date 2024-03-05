using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GiayDep.Models
{
    public partial class NhaSanXuat
    {
        public NhaSanXuat()
        {
            NhaCungCaps = new HashSet<NhaCungCap>();
        }

        public int Idnhasx { get; set; }

        [Required(ErrorMessage = "Tên nhà sản xuất không được để trống.")]
        public string Tennhasx { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống.")]
        public string Diachi { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        public string Sđt { get; set; }

        [Required(ErrorMessage = "Email không được để trống.")]
        public string Email { get; set; }

        public virtual ICollection<NhaCungCap> NhaCungCaps { get; set; }
    }
}
