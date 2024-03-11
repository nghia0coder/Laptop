using Microsoft.AspNetCore.Identity;

namespace Laptop.Models
{
    public class AppUserModel : IdentityUser
    {
        public string Address { get; set; }

        public virtual ICollection<HoaDon> HoaDons { get; } = new List<HoaDon>();
    }
}
