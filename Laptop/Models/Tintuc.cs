using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laptop.Models
{
    public partial class Tintuc
    {
        [Key]
        public int PostID { get; set; }
        public string? Title { get; set; }
        public string? Contentspreview { get; set; }
        public string? Contents { get; set; }
        public string? Thumburl { get; set; }
        public bool Status { get; set; }
        public int? Author { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool Hot { get; set; }
        public bool New { get; set; }
        public int? EmployeeId { get; set; }
        public int? BrandId { get; set; }

        public virtual Brand? Brand { get; set; }
        public virtual ICollection<PostComment> PostComments { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual Employee? Employee { get; set; }

        [NotMapped]
        public IFormFile? Img1 { get; set; }

    }
}
