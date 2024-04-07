using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class ProductComment
    {   
        public ProductComment() 
        {
            ProductComments = new HashSet<ProductComment>();
        }
            

        public int CommentId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Detail { get; set; }
        public bool? Status { get; set; }
        public int? ProductId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<ProductComment> ProductComments { get; set; }

        public virtual Product? Product { get; set; }

    }
}
