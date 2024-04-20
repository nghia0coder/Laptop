using MessagePack;
using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class ProductComment
    {
        public int CommentId { get; set; }
        public string? Detail { get; set; }
        public bool? Status { get; set; }
        public int? ProductId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int Rating { get; set; }

        public virtual Customer? CreatedByNavigation { get; set; }
        public virtual Product? Product { get; set; }
    }
}
