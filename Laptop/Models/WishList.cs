using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class WishList
    {
        public int WishlistId { get; set; }
        public int? CustomerId { get; set; }
        public int? ProductId { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual ProductVariation? Product { get; set; }
    }
}
