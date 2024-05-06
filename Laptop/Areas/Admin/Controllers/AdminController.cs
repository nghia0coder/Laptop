using Laptop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Laptop.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AdminController : Controller
    {
		private UserManager<AppUserModel> _userManager;
		private SignInManager<AppUserModel> _signInManager;
		private readonly LaptopContext _context;
		public AdminController(UserManager<AppUserModel> userManager, SignInManager<AppUserModel> signInManager, LaptopContext context)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_context = context;
		}
		[Authorize(Roles ="Manager, Staff")]
        public IActionResult Index()
        {
            int brandCount = _context.Brands.Count();
            ViewBag.BrandCount = brandCount;

            int colorCount = _context.Colors.Count();
            ViewBag.ColorCount = colorCount;

            int ramCount = _context.Rams.Count();
            ViewBag.RamCount = ramCount;

            int ssdCount = _context.Ssds.Count();
            ViewBag.SSDCount = ssdCount;

            int voucherCount = _context.Vouchers.Count();
            ViewBag.VoucherCount = voucherCount;

            int supplyCount = _context.Suppliers.Count();
            ViewBag.SupplierCount = supplyCount;

            int productCount = _context.Products.Count();
            ViewBag.ProductCount = productCount;

            int productitemCount = _context.ProductItems.Count();
            ViewBag.PICount = productitemCount;

            int categoryCount = _context.Categories.Count();
            ViewBag.CategoryCount = categoryCount;

            int invoiceCount = _context.Invoices.Count();
            ViewBag.InvoiceCount = invoiceCount;

            int provarCount = _context.ProductVariations.Count(p => p.QtyinStock < 5);
            ViewBag.ProVarCount = provarCount;

            return View();
        }
        public async Task<IActionResult> Login()
        {

            return View();
        }
        [HttpPost]
		public async Task<IActionResult> Login(UserModel user)
		{
			if (user.UserName != "admin@gmail.com" && user.UserName != "staff@gmail.com")
			{
				return View(user);
			}	
			Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, false, false);
			if (result.Succeeded)
			{
				return RedirectToAction("Index", "Admin");
			}
			ModelState.AddModelError("", "Error");
			return RedirectToAction("Index", "Admin");
		}
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
			return Redirect("/Home/Index");
        }
        [HttpPost]
        public SalesDataViewModel GetSalesData()
        {
            List<string> labels = new List<string>();
            List<long?> total = new List<long?>();
          
          
                for (int month = 1; month <= 12; month++)
                {
                    labels.Add(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month));

                    var totalRevenue = _context.Orders
                        .Where(order => order.OrderDate.Month == month)
                        .Sum(order => order.PriceTotal);
                    total.Add(totalRevenue);
                }


            return new SalesDataViewModel { Labels = labels, TotalRevenue = total };


        }
        [HttpPost]
        public SalesDataViewModel GetSalesPieData(int? monthSale)
        {

            List<string> labels = new List<string>();
            List<long?> total = new List<long?>();
            var brands = _context.Brands.ToList();

            foreach (var brand in brands)
            {
                labels.Add(brand.BrandName);

                var rev = _context.OrdersDetails
                 .Where(n => n.Order.OrderDate.Month == monthSale && n.ProductVar.ProductItems.Product.Brand == brand.BrandId)
                 .Sum(n => n.Order.PriceTotal);
                total.Add(rev);

            }

            return new SalesDataViewModel { Labels = labels, TotalRevenue = total };
        }

        public class SalesDataViewModel
        {
            public List<string> Labels { get; set; }
            public List<long?> TotalRevenue { get; set; }
        }


    }
}
