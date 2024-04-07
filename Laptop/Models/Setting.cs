using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laptop.Models
{
    public partial class Setting
    {
        public int Id { get; set; }
        [DisplayName("Hình Ảnh")]
        public string? ImgUrl { get; set; }
        public string? Contents { get; set; }

        [NotMapped]
        public IFormFile Img {  get; set; }


    }
}
