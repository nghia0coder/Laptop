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
using Laptop.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace Laptop.Controllers
{
	public class ShoppingCart : Controller
	{
		private readonly LaptopContext _context;
        private readonly IVnPayService _vnPayService;
        private IMomoService _momoService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private UserManager<AppUserModel> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SendMailService _sendMailService;
      
		public ShoppingCart(LaptopContext context, IHttpContextAccessor httpContextAccessor, UserManager<AppUserModel> userManager,SendMailService sendMailService, IMomoService momoService, IVnPayService vnPayService)
		{
			_context = context;
			_httpContextAccessor = httpContextAccessor;
            _sendMailService = sendMailService;
			_momoService = momoService;
            _vnPayService = vnPayService;
			_userManager = userManager;
        }


		public async Task<IActionResult> IndexAsync()
		{
			List<CartItemsModel> cartItems = HttpContext.Session.GetJson<List<CartItemsModel>>("Cart") ?? new List<CartItemsModel>();
			Item cart = new()
			{
				CartItems = cartItems,
				Quanity = cartItems.Count(),
				Total = cartItems.Sum(x => x.Quanity  * x.Price)
			};


            var user = await GetCurrentUserAsync();
         

			var customerAddress = _context.CustomerAddresses.Where(n => n.CustomerId == user).ToList();

	

            ViewBag.TongTien = cart.Total;
			ViewBag.TongQuanity = cart.Quanity;
            ViewData["Address"] = new SelectList(customerAddress, "CustomerAddressId", "AddressLine");
            return View(cart);
		}

        private async Task<int> GetCurrentUserAsync()
        {	
			var user = await _userManager.GetUserAsync(HttpContext.User);
			if (user == null)
			{
				return 0;
			}	
            var customerId = _context.Customers.Where(n => n.AccountId == user.Id).Select(n => n.CustomerId).FirstOrDefault();

            if (customerId == 0)
            {
                customerId = _context.Employees.Where(n => n.AccountId == user.Id).Select(n => n.EmployeeId).FirstOrDefault();
            }
			return customerId;
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

			if (quantity < 0)
			{
				return BadRequest("Số lượng không hợp lệ");
			}
			if (quantity == null || quantity == 0)
			{
				quantity = 1;
			}
			List<CartItemsModel> cart = HttpContext.Session.GetJson<List<CartItemsModel>>("Cart") ?? new List<CartItemsModel>();
			CartItemsModel cartItems = cart.Where(c => c.ProductID == id.ProductVarId).FirstOrDefault();
			if (cartItems == null)
			{
				if (quantity > productVariation.QtyinStock)
				{
					return BadRequest("Số lượng vượt quá số lượng trong kho!");

				}
				else
				{
					cart.Add(new CartItemsModel(productVariation, quantity));
				}
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
			return Redirect(strURL);
		}
        public async Task<IActionResult> AddToCart(int productID, string strURL,int quantity = 1)
        {

			var id = await _context.ProductVariations
					.Where(n=>n.ProductVarId == productID).FirstOrDefaultAsync();

            ProductVariation productVariation = await _context.ProductVariations
                .Include(n => n.Ram)
                .Include(n => n.Ssd)
                .Include(n => n.ProductItems)
                .Include(n => n.ProductItems.Color)
                .Include(n => n.ProductItems.Product)
                .FirstOrDefaultAsync(n => n.ProductVarId == productID);

       
            List<CartItemsModel> cart = HttpContext.Session.GetJson<List<CartItemsModel>>("Cart") ?? new List<CartItemsModel>();
            CartItemsModel cartItems = cart.Where(c => c.ProductID == id.ProductVarId).FirstOrDefault();
            if (cartItems == null)
            {
                if (quantity > productVariation.QtyinStock)
                {
                    return BadRequest("Số lượng vượt quá số lượng trong kho!");

                }
                else
                {
                    cart.Add(new CartItemsModel(productVariation, quantity));
                }
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
			TempData["error"] = "Add To Cart Success";
            return RedirectToAction("Index");
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
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
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

        public async Task<IActionResult> DatHangAsync(string id, string total,string address)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var customerId = _context.Customers.Where(n => n.AccountId == userId).Select(n => n.CustomerId).FirstOrDefault();
            // Check if the shopping cart session exists


            // Add a new order
            Order ddh = new Order();
            ddh.OrderId = id;
            ddh.OrderDate = DateTime.Now;
            ddh.OrderStatus = 1;
            ddh.StatusPayment = true;
            ddh.CustomerId = customerId;
			ddh.DeliveryAddress = address;
            ddh.PriceTotal = int.Parse(total);
            _context.Orders.Add(ddh);
            _context.SaveChanges();

            List<string> sanphams = new List<string>();
            // Add order details
            List<CartItemsModel> cart = HttpContext.Session.GetJson<List<CartItemsModel>>("Cart") ?? new List<CartItemsModel>();

            if (cart == null)
            {
                return Content("jdsjad");
            }
            foreach (var item in cart)
            {
                OrdersDetail ctdh = new OrdersDetail();
                ctdh.OrderId = ddh.OrderId;
                ctdh.ProductVarId = item.ProductID;
                ctdh.Quanity = item.Quanity;

                var product = _context.ProductVariations.FirstOrDefault(x => x.ProductVarId == ctdh.ProductVarId);

                product.QtyinStock -= item.Quanity;


                _context.OrdersDetails.Add(ctdh);

                await _context.Entry(ctdh).Reference(p => p.ProductVar).LoadAsync();
                await _context.Entry(ctdh.ProductVar).Reference(pv => pv.ProductItems).LoadAsync();
                await _context.Entry(ctdh.ProductVar.ProductItems).Reference(pv => pv.Product).LoadAsync();
                await _context.Entry(ctdh.ProductVar.ProductItems).Reference(pv => pv.Color).LoadAsync();
                await _context.Entry(ctdh.ProductVar).Reference(pv => pv.Ram).LoadAsync();
                await _context.Entry(ctdh.ProductVar).Reference(pv => pv.Ssd).LoadAsync();

                string productInfo = $"Name: {ctdh.ProductVar.ProductItems.Product.ProductName} {ctdh.ProductVar.Ram.RamName} " +
                  $"{ctdh.ProductVar.Ssd.Ssdname},Màu : {ctdh.ProductVar.ProductItems.Color.ColorName}, Quantity: {ctdh.Quanity} , Price: {ctdh.ProductVar.Price} VNĐ";
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
            HttpContext.Session.Remove("Cart");
            return PartialView("Success", ddh);

        }
        public async Task<IActionResult> Payment(string total,int addressId)
		{
            var customerId = await GetCurrentUserAsync();

			var customerAddress = await _context.CustomerAddresses.FindAsync(addressId);

            // Add a new order
            Order ddh = new Order();
			ddh.OrderId = DateTime.UtcNow.Ticks.ToString();
			ddh.OrderDate = DateTime.Now;
            ddh.OrderStatus = 1;
            ddh.StatusPayment = false;
            ddh.CustomerId = customerId;
			ddh.DeliveryAddress = customerAddress.AddressLine;
			ddh.PriceTotal = int.Parse(total);
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

				var product = _context.ProductVariations.FirstOrDefault(x => x.ProductVarId == ctdh.ProductVarId);

				product.QtyinStock -= item.Quanity;


				_context.OrdersDetails.Add(ctdh);

				await _context.Entry(ctdh).Reference(p => p.ProductVar).LoadAsync();
				await _context.Entry(ctdh.ProductVar).Reference(pv => pv.ProductItems).LoadAsync();
				await _context.Entry(ctdh.ProductVar.ProductItems).Reference(pv => pv.Product).LoadAsync();
				await _context.Entry(ctdh.ProductVar.ProductItems).Reference(pv => pv.Color).LoadAsync();
				await _context.Entry(ctdh.ProductVar).Reference(pv => pv.Ram).LoadAsync();
				await _context.Entry(ctdh.ProductVar).Reference(pv => pv.Ssd).LoadAsync();

				string productInfo = $"Name: {ctdh.ProductVar.ProductItems.Product.ProductName} {ctdh.ProductVar.Ram.RamName} " +
				  $"{ctdh.ProductVar.Ssd.Ssdname},Màu : {ctdh.ProductVar.ProductItems.Color.ColorName}, Quantity: {ctdh.Quanity} , Price: {ctdh.ProductVar.Price} VNĐ";
				sanphams.Add(productInfo);
			}
			_context.SaveChanges();

			var mailContent = new MailContent
			{
				To = "nghialmao@gmail.com",
				Subject = "Đơn hàng mới",
				Body = "Thông tin sản phẩm:\n" + string.Join("\n", sanphams)
			};
			await _sendMailService.SendMailAsync(mailContent);
			HttpContext.Session.Remove("Cart");
			return PartialView("Success", ddh);

		}
		public async Task<IActionResult> CreatePaymentUrl(long total)
        {
            var response = await _momoService.CreatePaymentAsync(total);
            return Redirect(response.PayUrl);
        }
        public IActionResult CreatePaymentVNPAYUrl(long total)
        {
            var url = _vnPayService.CreatePaymentUrl(total, HttpContext);

            return Redirect(url);
        }
		[HttpGet]
		public IActionResult PaymentCallBack()
		{
			var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
			return RedirectToAction("DatHang", "ShoppingCart", new { id = response.OrderId, total = response.Amount });

		}

		public IActionResult PaymentCallbackVPN()
		{
			var response = _vnPayService.PaymentExecute(HttpContext.Request.Query);
			return RedirectToAction("DatHang", "ShoppingCart", new { id = response.OrderId, total = response.OrderDescription});
			
        }
		public JsonResult ApplyVoucher(string voucherCode,long total)
        {
			var voucher = _context.Vouchers.Find(voucherCode);

			if (voucher == null)
			{
				return Json(new { success = false, message = "Mã giảm giá không hợp lệ." });
			}
            long? dicount = (total * voucher.Discount) / 100;

            long? newtotal = total - dicount;
            return Json(new { success = true, message = "Áp dụng mã giảm giá thành công!", newTotal = newtotal ,discount = dicount});
        }
       

        public long? TinhTongTien()
		{
			List<Item> cart = HttpContext.Session.GetJson<List<Item>>("Cart");
			if (cart == null)
			{
				return 0;
			}
			return cart.Sum(n => n.Total);
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> WishList(int pg = 1)
		{
			var customerId = await GetCurrentUserAsync();

			var wishList = await _context.WishLists.Where(n => n.CustomerId == customerId)
				.Include(n => n.Product)
					.ThenInclude(n => n.ProductItems)
				.Include(n => n.Product)
					.ThenInclude(n => n.ProductItems)
						.ThenInclude(n => n.Product)
							.ThenInclude(n => n.Category)
				.ToListAsync();

			if (wishList == null)
			{
				return NotFound();
			}

            const int pageSize = 5;



            if (pg < 1)
                pg = 1;

            var totalProduct = await _context.WishLists.Where(n => n.CustomerId == customerId).CountAsync();

            var pager = new Page(totalProduct, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            var data = wishList.Skip(recSkip).Take(pageSize).ToList();

            ViewBag.Page = pager;
            return View(data);
		}

		public async Task<IActionResult> UserOrders(string orderid, int id, int pg = 1)
		{
			const int pageSize = 5;
			if (orderid == null)
			{

				var customerId = await GetCurrentUserAsync();



				var orders = await _context.Orders.Where(n => n.CustomerId == customerId && n.OrderStatus == id)
					.Include(o => o.StatusNaviagtion)
					.OrderByDescending(n => n.OrderDate)
					.ToListAsync();


				if (pg < 1)
					pg = 1;

				var totalCate = await _context.Orders.Where(n => n.CustomerId == customerId && n.OrderStatus == id).CountAsync();

				var pager = new Page(totalCate, pg, pageSize);

				int recSkip = (pg - 1) * pageSize;

				var data = orders.Skip(recSkip).Take(pageSize).ToList();

				ViewBag.Page = pager;

				return View(data);
			}
			else
			{

				var customerId = await GetCurrentUserAsync();


				var orders = await _context.Orders.Where(n => n.CustomerId == customerId && n.OrderStatus == id && n.OrderId.ToString().Contains(orderid))
					.Include(o => o.StatusNaviagtion)
					.OrderByDescending(n => n.OrderDate)
					.ToListAsync();

				return View(orders);
			}

		}
        [HttpGet]
    
        public async Task<IActionResult> CancelConfirm(string id, int? detail)
        {
            var order = await _context.OrdersDetails
                .Include(n => n.ProductVar.ProductItems)
                    .ThenInclude(n => n.Product)
                        .ThenInclude(n => n.BrandNavigation)
                .Include(n => n.ProductVar.Ram)
				.Include(n => n.ProductVar.Ssd)
				.Include(n => n.ProductVar.ProductItems)
                    .ThenInclude(n => n.Product.Category)
                 .Include(n => n.ProductVar.ProductItems)
                    .ThenInclude(n => n.Color)
                .Include(n => n.Order)
                    .ThenInclude(n => n.StatusNaviagtion)
                .Where(n => n.OrderId == id)
                .ToListAsync();

            ViewBag.Detail = detail;
            return View(order);
        }




    }
}
