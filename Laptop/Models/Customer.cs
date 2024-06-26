﻿using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class Customer
    {
        public Customer()
        {
            CustomerAddresses = new HashSet<CustomerAddress>();
            Orders = new HashSet<Order>();
            PostComments = new HashSet<PostComment>();
            ProductComments = new HashSet<ProductComment>();
            WishLists = new HashSet<WishList>();
        }

        public int CustomerId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? AccountId { get; set; }

        public virtual AppUserModel? Account { get; set; }
        public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<PostComment> PostComments { get; set; }
        public virtual ICollection<ProductComment> ProductComments { get; set; }
  
        public virtual ICollection<WishList> WishLists { get; set; }
    }
}
