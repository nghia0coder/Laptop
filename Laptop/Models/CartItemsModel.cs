using Laptop.Models;

namespace Laptop.Models
{
	public class CartItemsModel
	{
		public int ProductID { get; set; }
		public string? ProductName { get; set; }
		public int? Quanity { get; set; }
		public int? Price { get; set; }
		public int? Total
		{
			get { return Price * Quanity; }
		}
		public string? ram { get; set; }
		public string? SSD { get; set; }

		public string? Color { get; set; }

		public string? HinhAnh { get; set; }

		public string? Hang { get; set; }

		public CartItemsModel() { }

		// Constructor theo id (dùng cho trường hợp chỉ có sl=1)
		public CartItemsModel(ProductVariation Product, int quantity)
		{
			ProductID = Product.ProductVarId;
			ProductName = Product.ProductItems.Product.ProductName;
			Quanity = Product.QtyinStock;
			Price = Product.Price;
			HinhAnh = Product.ProductItems.Image1;
			Color = Product.ProductItems.Color.ColorName;
			ram = Product.Ram.RamName;
			SSD = Product.Ssd.Ssdname;
			Quanity = quantity;
			//HinhAnh = Product.ProductItems.Image1;
		}
	}
}
