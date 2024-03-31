using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laptop.Models
{
    public partial class News
    {
        public int PostId { get; set; }
        public string? Title { get; set; }
        public string? ContentPreview { get; set; }
        public string? Contents { get; set; }
        public string? ThumbUrl { get; set; }
        public bool Status { get; set; }
        public string? Author { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool Hot { get; set; }
        public bool New { get; set; }
        public int? BrandId { get; set; }

		[NotMapped]
		public IFormFile Img1 { get; set; }

		public virtual Brand? Brand { get; set; }
    }
}
