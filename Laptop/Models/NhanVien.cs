using System;
using System.Collections.Generic;

namespace GiayDep.Models
{
    public partial class NhanVien
    {
        public string Idnhanvien { get; set; } = null!;
        public string? Tennv { get; set; }
        public string? Diachi { get; set; }
        public string? Email { get; set; }
        public DateTime? Ngaysinh { get; set; }
        public string? Gioitinh { get; set; }
        public string? Sdt { get; set; }
    }
}
