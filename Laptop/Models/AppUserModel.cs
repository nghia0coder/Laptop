using Microsoft.AspNetCore.Identity;

namespace Laptop.Models
{
    public class AppUserModel : IdentityUser
    {
   
		public virtual ICollection<Customer> Customers { get; } = new List<Customer>();

		public virtual ICollection<Employee> Employees { get; } = new List<Employee>();

        

        public virtual ICollection<Tintuc> Post { get; set; } = new List<Tintuc>();


    }
}
