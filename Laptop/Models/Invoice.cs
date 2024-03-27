using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class Invoice
    {
        public Invoice()
        {
            InvoiceDetails = new HashSet<InvoiceDetail>();
        }

        public int InvoiceId { get; set; }
        public int SupplierId { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Supplier Supplier { get; set; } = null!;
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    }
}
