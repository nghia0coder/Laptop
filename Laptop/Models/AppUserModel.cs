using Microsoft.AspNetCore.Identity;

namespace Laptop.Models
{
    public class AppUserModel : IdentityUser
    {
        public string Address { get; set; }

		public virtual ICollection<Order> Orders { get; } = new List<Order>();
        public virtual ICollection<Tintuc> Tintucs { get; } = new List<Tintuc>();

        public virtual ICollection<ProductComment> Comments { get; } = new List<ProductComment>();
    }
}
