using Laptop.Models;

namespace Laptop.Models
{
	public class CartItemsModel
	{
		public int ProductID { get; set; }
		public string ProductName { get; set; }
		public int? Quanity { get; set; }
		public int? Price { get; set; }
		public int? Total
		{
			get { return Price * Quanity; }
		}
		public int? Size { get; set; }

		public string Color { get; set; }

		public string HinhAnh { get; set; }

		public string Hang { get; set; }

		public CartItemsModel() { }

		// Constructor theo id (dùng cho trường hợp chỉ có sl=1)
		public CartItemsModel(ProductVariation Product, int quantity)
		{
			ProductID = Product.ProductVarId;
			ProductName = Product.ProductItems.Product.ProductName;
			Quanity = Product.QtyinStock;
			HinhAnh = Product.ProductItems.Image1;
			//Size = Product.
            //Ssd = Product.ProductItems.Color.ColorName;
            //Quanity = quantity;
			//HinhAnh = Product.Hinhanh1;
		}
	}
}
