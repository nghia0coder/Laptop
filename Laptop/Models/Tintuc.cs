using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laptop.Models
{
    public class Tintuc
    {
        [Key]
        public int PostID { get; set; }
        public string? Title { get; set; }
        public string? Contentspreview { get; set; }
        public string? Contents { get; set; }
        public string? Thumburl { get; set; }
        public bool Status { get; set; }
        public string? Author { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool Hot { get; set; }
        public bool New { get; set; }
        public int BrandId { get; set; }

        public virtual Brand? Brand { get; set; }

        [NotMapped]
        public IFormFile Img1 { get; set; }

}
}