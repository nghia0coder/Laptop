using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Invoices = new HashSet<Invoice>();
            Orders = new HashSet<Order>();
        }

        public int EmployeeId { get; set; }
        public string? Name { get; set; }
        public string? Identification { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? AccountId { get; set; }

        public virtual AppUserModel? Account { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<Tintuc> Post { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
