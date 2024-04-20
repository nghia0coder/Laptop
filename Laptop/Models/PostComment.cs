using System;
using System.Collections.Generic;

namespace Laptop.Models
{
    public partial class PostComment
    {
        public int PostCommentId { get; set; }
        public string Comment { get; set; } = null!;
        public int PostId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual Tintuc Post { get; set; } = null!;
    }
}
