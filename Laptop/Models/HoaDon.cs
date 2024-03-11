using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class HoaDon
    {
        public HoaDon()
        {
            CtHoaDons = new HashSet<CtHoaDon>();
        }

        public int Idhoadon { get; set; }
        public DateTime? Ngaythanhtoan { get; set; }
        public bool Tinhtranggiaohang { get; set; }
        public DateTime? Ngaygiao { get; set; }
        public bool Dathanhtoan { get; set; }
        public bool? Dahuy { get; set; }
        public bool? Daxoa { get; set; }
        public string Makh { get; set; } = null!;
        public int? Khuyenmai { get; set; }

        public virtual AppUserModel MakhNavigation { get; set; } = null!;
        public virtual ICollection<CtHoaDon> CtHoaDons { get; set; }
    }
}
