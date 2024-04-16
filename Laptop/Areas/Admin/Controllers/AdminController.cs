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

    }
}
