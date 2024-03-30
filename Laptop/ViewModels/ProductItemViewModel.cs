using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace Laptop.ViewModels
{
    public class ProductItemViewModel
    {
        public int ProductItemsId { get; set; }
        public string ProductCode { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public int Ram {  get; set; }
        public int Ssd { get; set; }

        public int Price { get; set; }
        public int Size { get; set; }
        public int ProductId { get; set; }

       

        [NotMapped]
        public IFormFile Img1 { get; set; }
        [NotMapped]
        public IFormFile Img2 { get; set; }
        [NotMapped]
        public IFormFile Img3 { get; set; }
        [NotMapped]
        public IFormFile Img4 { get; set; }

    }
}
