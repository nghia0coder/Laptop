using System;
using System.Collections.Generic;

namespace GiayDep.Models
{
    public partial class Right
    {
        public Right()
        {
            MembershipRights = new HashSet<MembershipRight>();
        }

        public string Idright { get; set; } = null!;
        public string? NameRight { get; set; }

        public virtual ICollection<MembershipRight> MembershipRights { get; set; }
    }
}
