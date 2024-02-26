using System;
using System.Collections.Generic;

namespace GiayDep.Models
{
    public partial class MembershipRight
    {
        public int MaLoaiTv { get; set; }
        public string Idright { get; set; } = null!;
        public string? Note { get; set; }

        public virtual Right IdrightNavigation { get; set; } = null!;
        public virtual MembershipType MaLoaiTvNavigation { get; set; } = null!;
    }
}
