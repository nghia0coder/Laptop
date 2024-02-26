using System;
using System.Collections.Generic;

namespace GiayDep.Models
{
    public partial class MembershipType
    {
        public MembershipType()
        {
            MembershipRights = new HashSet<MembershipRight>();
            Memberships = new HashSet<Membership>();
        }

        public int MaLoaiTv { get; set; }
        public string? TenLoai { get; set; }
        public int? KhuyenMai { get; set; }

        public virtual ICollection<MembershipRight> MembershipRights { get; set; }
        public virtual ICollection<Membership> Memberships { get; set; }
    }
}
