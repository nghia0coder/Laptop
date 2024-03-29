using Laptop.Models;
using Microsoft.AspNetCore.Mvc;
using Laptop.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Laptop.Repository;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Laptop.Models;
using Laptop.ViewModels;
using Laptop.Service;
using Laptop.Interface;

namespace Laptop.Controllers
{
	public class ShoppingCart : Controller
	{
		private readonly LaptopContext _context;
   
        private readonly IHttpContextAccessor _httpContextAccessor;
        private UserManager<AppUserModel> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SendMailService _sendMailService;
      
		public ShoppingCart(LaptopContext context, IHttpContextAccessor httpContextAccessor, SendMailService sendMailService)
		{
			_context = context;
			_httpContextAccessor = httpContextAccessor;
            _sendMailService = sendMailService;

        }


		public IActionResult Index()
		{
			List<CartItemsModel> cartItems = HttpContext.Session.GetJson<List<CartItemsModel>>("Cart") ?? new List<CartItemsModel>();
			Item cart = new()
			{
				CartItems = cartItems,
				Quanity = cartItems.Count(),
				Total = cartItems.Sum(x => (x.Quanity ?? 0) * (x.Price ?? 0))
			};
			ViewBag.TongTien = cart.Total;
			ViewBag.TongQuanity = cart.Quanity;
			return View(cart);
		}

		public async Task<IActionResult> ThemGioHang(int masp,int ramId,int ssdId ,int quantity, string strURL)
		{

			var id =await _context.ProductVariations
				.Where(n => n.ProductItemsId == masp && n.RamId == ramId && n.Ssdid == ssdId)
				.FirstOrDefaultAsync();

            ProductVariation productVariation = await _context.ProductVariations
				.Include(n => n.Ram)
				.Include(n => n.Ssd)
				.Include(n => n.ProductItems)
				.Include(n => n.ProductItems.Color)
				.Include(n => n.ProductItems.Product)
				.FirstOrDefaultAsync(n => n.ProductItems.ProductItemsId == masp && n.RamId == ramId && n.Ssdid == ssdId);


            if (quantity == null)
			{
				quantity = 1;
			}
			List<CartItemsModel> cart = HttpContext.Session.GetJson<List<CartItemsModel>>("Cart") ?? new List<CartItemsModel>();
			CartItemsModel cartItems = cart.Where(c => c.ProductID == id.ProductVarId).FirstOrDefault();
			if (cartItems == null)
			{

				cart.Add(new CartItemsModel(productVariation, quantity));
			}
			else
			{	
				cartItems.Quanity += quantity;
				if (cartItems.Quanity > productVariation.QtyinStock) 
				{
					cartItems.Quanity -= quantity;
					return BadRequest("Số lượng vượt quá số lượng trong kho!");

				}
			}
			HttpContext.Session.SetJson("Cart", cart);
			TempData["success"] = "Thêm vào giỏ hàng thành công";
			return Redirect(strURL);
		}
		public async Task<IActionResult> Decrease(int Id)
		{

			List<CartItemsModel> cart = HttpContext.Session.GetJson<List<CartItemsModel>>("Cart");
			CartItemsModel cartItems = cart.Where(c => c.ProductID == Id).FirstOrDefault();
			if (cartItems.Quanity > 1)
			{
				--cartItems.Quanity;
			}
			else
			{
				cart.RemoveAll(p => p.ProductID == Id);
			}
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Increase(int Id)
		{

			List<CartItemsModel> cart = HttpContext.Session.GetJson<List<CartItemsModel>>("Cart");
			CartItemsModel cartItems = cart.Where(c => c.ProductID == Id).FirstOrDefault();
			var product = _context.ProductVariations.Where(n => n.ProductVarId == cartItems.ProductID).FirstOrDefault();
			if (cartItems.Quanity >= 1)
			{	
				if (cartItems.Quanity >= product.QtyinStock)
				{
					TempData["success"] = "Vượtt quá số lượng tồn";
					return RedirectToAction("Index");
				}	
				cartItems.Quanity += 1;
			}
			HttpContext.Session.SetJson("Cart", cart);
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Removed(int Id)
		{

			List<CartItemsModel> cart = HttpContext.Session.GetJson<List<CartItemsModel>>("Cart");
			cart.RemoveAll(n => n.ProductID == Id);
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			TempData["success"] = "Đã Xóa Sản Phẩm Ra Khỏi Giỏ Hàng";
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Clear(int Id)
		{
			TempData["success"] = "Đã Xóa Tất Cả Sản Phẩm Ra Khỏi Giỏ Hàng";
			HttpContext.Session.Remove("Cart");
			return RedirectToAction("Index");
		}
		public IActionResult Success()
		{
			return View();
		}
		public async Task<IActionResult> DatHangAsync()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			// Check if the shopping cart session exists

			// Add a new order
			Order ddh = new Order();

			ddh.OrderDate = DateTime.Now;
			ddh.Delivered = false;
			ddh.Status = false;
			ddh.CustomerId = userId;
			_context.Orders.Add(ddh);
			_context.SaveChanges();

            List<string> sanphams = new List<string>();
            // Add order details
            List<CartItemsModel> cart = HttpContext.Session.GetJson<List<CartItemsModel>>("Cart") ?? new List<CartItemsModel>();
			foreach (var item in cart)
			{
				OrdersDetail ctdh = new OrdersDetail();
                ctdh.OrderId = ddh.OrderId;
				ctdh.ProductVarId = item.ProductID;
				ctdh.Quanity = item.Quanity;
				ctdh.Price = item.Total;
         
                _context.OrdersDetails.Add(ctdh);
     
                await _context.Entry(ctdh).Reference(p => p.ProductVar).LoadAsync();
                await _context.Entry(ctdh.ProductVar).Reference(pv => pv.ProductItems).LoadAsync();
                await _context.Entry(ctdh.ProductVar.ProductItems).Reference(pv => pv.Product).LoadAsync();
                await _context.Entry(ctdh.ProductVar.ProductItems).Reference(pv => pv.Color).LoadAsync();
                await _context.Entry(ctdh.ProductVar).Reference(pv => pv.Ram).LoadAsync();
                await _context.Entry(ctdh.ProductVar).Reference(pv => pv.Ssd).LoadAsync();

                string productInfo = $"Name: {ctdh.ProductVar.ProductItems.Product.ProductName} {ctdh.ProductVar.Ram.RamName} " +
                  $"{ctdh.ProductVar.Ssd.Ssdname},Màu : {ctdh.ProductVar.ProductItems.Color.ColorName}, Quantity: {ctdh.Quanity} , Price: {ctdh.Price} VNĐ";
                sanphams.Add(productInfo);
            }
			_context.SaveChanges();

            var mailContent = new MailContent
            {
                To = "nghialmao@gmail.com",
                Subject = "Đơn hàng mới",
                Body = "Thông tin sản phẩm:\n" + string.Join("\n", sanphams)
            };
            await _sendMailService.SendMail(mailContent);
            HttpContext.Session.Remove("Cart"); // Clear the shopping cart session

			// Redirect to the order success page
			return RedirectToAction("Success");
		}
		public int? TinhTongTien()
		{
			List<Item> cart = HttpContext.Session.GetJson<List<Item>>("Cart");
			if (cart == null)
			{
				return 0;
			}
			return cart.Sum(n => n.Total);
		}


	}
}
