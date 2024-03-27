using Microsoft.AspNetCore.Identity;

namespace Laptop.Models
{
    public class AppUserModel : IdentityUser
    {
        public string Address { get; set; }

		public virtual ICollection<Order> Orders { get; } = new List<Order>();
	}
}
