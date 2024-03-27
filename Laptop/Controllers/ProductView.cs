using Laptop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Laptop.Controllers
{
	public class ProductView : Controller
	{
		private readonly LaptopContext _context;

		public ProductView(LaptopContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			return View();
		}

		public ActionResult Productstyle1Partial()
		{
			return PartialView();
		}
		public ActionResult Productstyle2Partial()
		{
			return PartialView();
		}
		[Route("post/{slug}-{id:int}")]
		public async Task<IActionResult> XemChiTiet(int? id)
		{
			if (id == null)
			{
				return BadRequest();
			}
			var sp = await _context.ProductVariations
				.Include(n => n.ProductItems.Product)
				.FirstOrDefaultAsync(n => n.ProductItemsId == id);

			ViewBag.ListSP = _context.ProductItems
				.Include(n => n.Product.Category)
				.Where(n => n.Product.CategoryId == sp.ProductItems.Product.CategoryId);

			if (sp == null)
			{
				return NotFound();
			}

			return View(sp);
		}

		[Route("Product/{slug}-{id:int}")]
		public IActionResult ProductCate(int? Id)
		{
			// Check if the parameter is null
			if (Id == null)
			{
				return BadRequest();
			}

			// Load products based on the specified criteria
			var lstSP = _context.Products
				.Where(n => n.BrandNavigation.BrandId == Id)
				.Include(n => n.Category)
				.GroupBy(n => n.ProductName)
				.Select(n => n.FirstOrDefault())
				.ToList();

			// Check if there are any products
			if (lstSP.Count() == 0)
			{
				return NotFound();
			}
			ViewBag.CategoryId = Id;

			// Return the view with paginated products
			return View(lstSP.OrderBy(n => n.ProductId));
		}

		public async Task<JsonResult> GetColorAsync(int id)
		{
			var list = await _context.ProductItems
				.Where(n => n.ProductId == id)
				.Include(n => n.Color)
				.ToListAsync();
			return Json(list);
		}
		public async Task<JsonResult> GetImages(int id)
		{
			var img = await _context.ProductItems.Where(n => n.ProductItemsId == id).FirstOrDefaultAsync();

			return Json(img);
		}
		public async Task<JsonResult> GetSizeAsync(int id)
		{
			var list = await _context.ProductVariations
				.Where(n => n.ProductItems.Product.ProductId == id)
				////.Include(n => n.Size)
				.ToListAsync();
			return Json(list);
		}
		public async Task<JsonResult> GetSizeByColorAsync(int id)
		{
			var list = await _context.ProductVariations
				.Where(n => n.ProductItemsId == id)
				////.Include(n => n.Size)
				.ToListAsync();
			return Json(list);
		}
	}
}
