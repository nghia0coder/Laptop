using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class ProductComment
    {   
        public ProductComment() 
        {
        
        }
            

        public int CommentId { get; set; }
        public string? Detail { get; set; }
        public bool? Status { get; set; }
        public int? ProductId { get; set; }
        public string? CreatedBy { get; set; }

        public int Ratings { get; set; }

        public DateTime? CreateDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Product? Product { get; set; }

        public virtual AppUserModel Customer { get; set;}

    }
}
