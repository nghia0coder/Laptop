using Laptop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PagedList.Core;
using System.Collections.Generic;
using System.Drawing.Printing;


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
		public async Task<IActionResult> XemChiTiet(int? id, int? page)
		{
			if (id == null)
			{
				return BadRequest();
			}

			var sp = await _context.ProductVariations
				.Include(n => n.ProductItems.Product.ProductComments)
					.ThenInclude(n => n.CreatedByNavigation)
				.Include(n => n.ProductItems.Product.Category)
				.Include(n => n.Ram)
				.Include(n => n.Ssd)
				.FirstOrDefaultAsync(n => n.ProductVarId == id);
			
			ViewBag.Comment = await _context.ProductComments
				.Include(n => n.CreatedByNavigation)
					.ThenInclude(n => n.Account)
				.Where(n => n.ProductId  == sp.ProductItems.ProductId)
				.ToListAsync();



			ViewBag.ListSP = _context.ProductVariations
				.Where(n => n.ProductItemsId == sp.ProductItemsId && n.ProductVarId != id)
				.Include(n => n.ProductItems)
				.Include(n => n.Ram)
				.Include(n => n.Ssd)
				.Include(n => n.ProductItems.Product.Category)
				.ToList();


			if (sp == null)
			{
				return NotFound();
			}

			return View(sp);
		}
		[HttpGet]
		public async Task<IActionResult> ProductSearch(string s, int categoryId)
		{	

			
			if (!string.IsNullOrEmpty(s))
			{	
				if (categoryId != 0)
				{
					var product = _context.Products
					.Where(x => x.ProductName.Contains(s) && x.CategoryId == categoryId)
					.Include(n => n.Category)
					.Include(x => x.ProductItems)
					.ThenInclude(x => x.ProductVariations)
					.ToList();
                    return View(product);
                }
				else
				{
                    var product = _context.Products
                    .Where(x => x.ProductName.Contains(s))
                    .Include(n => n.Category)
                    .Include(x => x.ProductItems)
                    .ThenInclude(x => x.ProductVariations)
                    .ToList();
                    return View(product);
                }	
				

			
			}
			return View();
		}

		[Route("Product/{slug}-{id:int}")]
		public IActionResult ProductBrand(int? Id)
		{
			// Check if the parameter is null
			if (Id == null)
			{
				return BadRequest();
			}

			// Load products based on the specified criteria
			var lstSP = _context.ProductVariations
				.Where(n => n.ProductItems.Product.BrandNavigation.BrandId == Id)
				.Include(n => n.ProductItems.Product.Category)
				.Include(n => n.ProductItems.Product.BrandNavigation)
				.GroupBy(n => n.ProductItems.Product.ProductName)
				.Select(group => group.First())
				.ToList();


			// Check if there are any products
			if (lstSP.Count() == 0)
			{
				return NotFound();
			}
			ViewBag.CategoryId = Id;

			// Return the view with paginated products
			return View(lstSP.OrderBy(n => n.ProductVarId));
		}
		[Route("category/{slug}-{id:int}")]
		public IActionResult ProductCate(int? Id)
		{
			// Check if the parameter is null
			if (Id == null)
			{
				return BadRequest();
			}

			// Load products based on the specified criteria
			var lstSP = _context.ProductVariations
				.Where(n => n.ProductItems.Product.Category.CategoryId == Id)
				.Include(n => n.ProductItems.Product.Category)
				.Include(n => n.ProductItems.Product.BrandNavigation)
				.GroupBy(n => n.ProductItems.Product.ProductName)
				.Select(group => group.First())
				.ToList();


			// Check if there are any products
			if (lstSP.Count() == 0)
			{
				return NotFound();
			}
			ViewBag.CategoryId = Id;

			// Return the view with paginated products
			return View(lstSP.OrderBy(n => n.ProductVarId));
		}

		public async Task<JsonResult> GetColorAsync(int id)
		{
			var list = await _context.ProductItems
				.Where(n => n.ProductId == id)
				.Include(n => n.Color)
				.ToListAsync();
			return Json(list);
		}
		public async Task<JsonResult> GetSizeAsync(int id)
		{
			var list = await _context.ProductVariations
				.Where(n => n.ProductItems.Product.ProductId == id)
				////.Include(n => n.Size)
				.ToListAsync();
			return Json(list);
		}
		public async Task<JsonResult> GetImages(int id)
		{
			var img = await _context.ProductItems.Where(n => n.ProductItemsId == id).FirstOrDefaultAsync();

			return Json(img);
		}
		public async Task<JsonResult> GetSizeByColorAsync(int id)
		{
			var list = await _context.ProductVariations
				.Where(n => n.ProductItemsId == id)
				////.Include(n => n.Size)
				.ToListAsync();
			return Json(list);
		}
		
		public IActionResult GetMoreRecord (int id,int page = 1, int pageSize = 3)
		{
			var record = _context.ProductComments.Where(n => n.ProductId == id).Skip((page - 1) * pageSize).Take(pageSize)
				.Include(n => n.CreatedByNavigation)
				.ToList();

			return PartialView("_CommentPartial", record);
		}
	

	}
}
