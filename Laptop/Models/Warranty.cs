using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class Warranty
    {
        public int WarrantyId { get; set; }
        public int? CustomerId { get; set; }
        public string? OrderId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Reason { get; set; }
        public string? Service { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Order? Order { get; set; }
    }
}
