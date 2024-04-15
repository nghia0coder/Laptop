using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Laptop.Models
{
    public partial class PostComment
    {
        public PostComment()
        {
            
        }
        [Key]
        public int PostCommentID { get; set; }
        public string Comment { get; set; }

        public int PostID { get; set; }

        public virtual Tintuc? Tintuc { get; set; }
        public DateTime CreatedDate { get; set; }

        public string CustomerId { get; set; }
        public virtual AppUserModel? Customer { get; set; }
    }
}
