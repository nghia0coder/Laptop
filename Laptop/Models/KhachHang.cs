using System;
using System.Collections.Generic;

namespace GiayDep.Models
{
    public partial class KhachHang
    {
        public int Idkhachhang { get; set; }
        public string Tenkh { get; set; } = null!;
        public string? Diachi { get; set; }
        public string? Email { get; set; }
        public string? Sđt { get; set; }
        public string? Gioitinh { get; set; }
        public int? Idtv { get; set; }

        public virtual Membership? IdtvNavigation { get; set; }
    }
}
