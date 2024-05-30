using Laptop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PagedList.Core;
using System.Collections.Generic;
using System.Drawing.Printing;
using Page = Laptop.Models.Page;


namespace Laptop.Controllers
{
	public class ProductView : Controller
	{
		private readonly LaptopContext _context;
		private readonly UserManager<AppUserModel> _userManager;

		public ProductView(LaptopContext context, UserManager<AppUserModel> userManager)
		{
			_context = context;
			_userManager = userManager;
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
		[Route("detail/{id:int}")]
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
		public IActionResult ProductSearch(string s, int categoryId)
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

		[Route("Product/{id:int}")]
		public IActionResult ProductBrand(int? Id, int pg = 1)
		{
			// Check if the parameter is null
			if (Id == null)
			{
				return BadRequest();
			}

			// Load products based on the specified criteria
			var lstSP = _context.ProductVariations
				.Where(n => n.ProductItems.Product.Brand == Id)
				.Include(n => n.ProductItems)
					.ThenInclude(n => n.Color)
				.Include(n => n.Ram)
				.Include(n => n.ProductItems)
					.ThenInclude(n => n.ProductVariations)
						.ThenInclude(n => n.Ssd)
				.Include(n => n.ProductItems.Product.Category)
				.Include(n => n.ProductItems.Product.BrandNavigation)
				.OrderBy(n => Guid.NewGuid())
				.ToList();

			ViewBag.Categories = _context.Categories.Include(n => n.Products).ToList();
			ViewBag.Brand = _context.Brands.Include(n => n.Products).ToList();


			const int pageSize = 6;
			if (pg < 1)
				pg = 1;

			var totalCate = _context.ProductVariations.Where(n => n.ProductItems.Product.Brand == Id).Count();

			var pager = new Page(totalCate, pg, pageSize);

			int recSkip = (pg - 1) * pageSize;

			var data = lstSP.Skip(recSkip).Take(pageSize).ToList();

			ViewBag.Page = pager;


			// Return the view with paginated products
			return View(data.OrderBy(n => n.Price));
		}
		[Route("category/{id:int}")]
		public IActionResult ProductCate(int? Id,int pg=1)
		{
			// Check if the parameter is null
			if (Id == null)
			{
				return BadRequest();
			}

			// Load products based on the specified criteria
			var lstSP = _context.ProductVariations
				.Where(n => n.ProductItems.Product.CategoryId == Id)
				.Include(n => n.ProductItems)
					.ThenInclude(n => n.Color)
				.Include(n => n.Ram)
				.Include(n => n.ProductItems)
					.ThenInclude(n => n.ProductVariations)
						.ThenInclude(n => n.Ssd)
				.Include(n => n.ProductItems.Product.Category)
				.Include(n => n.ProductItems.Product.BrandNavigation)
				.OrderBy(n => Guid.NewGuid())
				.ToList();

			ViewBag.Categories = _context.Categories.Include(n => n.Products).ToList();
			ViewBag.Brand = _context.Brands.Include(n => n.Products).ToList();


			const int pageSize = 6;
			if (pg < 1)
				pg = 1;

			var totalCate = _context.ProductVariations.Where(n => n.ProductItems.Product.CategoryId == Id).Count();

			var pager = new Page(totalCate, pg, pageSize);

			int recSkip = (pg - 1) * pageSize;

			var data = lstSP.Skip(recSkip).Take(pageSize).ToList();

			ViewBag.Page = pager;


			// Return the view with paginated products
			return View(data.OrderBy(n => n.Price));
		}
		private async Task<int> GetCurrentUserAsync()
		{
			var user = await _userManager.GetUserAsync(HttpContext.User);
			var customerId = _context.Customers.Where(n => n.AccountId == user.Id).Select(n => n.CustomerId).FirstOrDefault();

			if (customerId == 0)
			{
				customerId = _context.Employees.Where(n => n.AccountId == user.Id).Select(n => n.EmployeeId).FirstOrDefault();
			}
			return customerId;
		}
		[HttpPost]
		public async Task<IActionResult> AddWishList(int? id)
		{
			if(id == null)
			{
				return BadRequest();
			}
			var product = await _context.ProductVariations.FirstOrDefaultAsync(n => n.ProductVarId == id);

			if(product == null)
			{
				return NotFound();
			}

			var wish = new WishList()
			{
				ProductId = product.ProductVarId,
				CustomerId = await GetCurrentUserAsync(),
			};
			
			_context.WishLists.Add(wish);
			await _context.SaveChangesAsync();

			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> RemoveWishList(int? id)
		{
			if (id == null)
			{
				return BadRequest();
			}
			var product = await _context.ProductVariations.FirstOrDefaultAsync(n => n.ProductVarId == id);

			if (product == null)
			{
				return NotFound();
			}
			var user = await GetCurrentUserAsync();
			var wish = await _context.WishLists.FirstOrDefaultAsync(n => n.ProductId == id && n.CustomerId == user);

			if (wish == null)
			{
				return NotFound();
			}
			_context.WishLists.Remove(wish);
			await _context.SaveChangesAsync();
			return Ok();
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

        [HttpPost]
        public async Task<IActionResult> DeleteWishList(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var wish = await _context.WishLists.FindAsync(id);	

            if (wish == null)
            {
                return NotFound();
            }
           
            _context.WishLists.Remove(wish);
            await _context.SaveChangesAsync();
            return Ok();
        }
		[HttpGet]
		public IActionResult FilterByBrands(int brandid, int cateid)
		{
			var product = _context.ProductVariations.
				Where(n => n.ProductItems.Product.BrandNavigation.BrandId == brandid && n.ProductItems.Product.CategoryId == cateid)
				.Select(m => new
				{
					Brand = m.ProductItems.Product.Brand,
					Category = m.ProductItems.Product.Category.CategoryName,
					Description = m.ProductItems.Product.Description,
					Price = m.Price,
					ProductId = m.ProductItems.Product.ProductId,
					ProductName = m.ProductItems.Product.ProductName,
					Ram = m.Ram.RamName,
					Ssd = m.Ssd.Ssdname,
					Img = m.ProductItems.Image1,
									ProductVarId = m.ProductVarId
				}).OrderBy(n => Guid.NewGuid()).ToList();
			return new JsonResult(product);
		}

		[HttpGet]
		public IActionResult SortProducts(int sortBy, int? categoryId, int? brandId)
		{
			if (categoryId != null)
			{
				var productQuery = _context.ProductVariations
				.Where(n => n.ProductItems.Product.CategoryId == categoryId)
					.Select(m => new
					{
						Brand = m.ProductItems.Product.Brand,
						Category = m.ProductItems.Product.Category.CategoryName,
						Description = m.ProductItems.Product.Description,
						Price = m.Price,
						ProductId = m.ProductItems.Product.ProductId,
						ProductName = m.ProductItems.Product.ProductName,
						Img = m.ProductItems.Image1,
						Ram = m.Ram.RamName,
						Ssd = m.Ssd.Ssdname,
						ProductVarId = m.ProductVarId
					});

				if (sortBy == 1) // Sắp xếp giảm dần
				{
					productQuery = productQuery.OrderByDescending(n => n.Price);
				}
				else if (sortBy == 0) // Sắp xếp tăng dần
				{
					productQuery = productQuery.OrderBy(n => n.Price);
				}

				var product = productQuery.ToList();

				return new JsonResult(product);
			}
			else
			{
				var productQuery = _context.ProductVariations
						.Where(n => n.ProductItems.Product.Brand == brandId)
										.Select(m => new
										{
											Brand = m.ProductItems.Product.Brand,
											Category = m.ProductItems.Product.Category.CategoryName,
											Description = m.ProductItems.Product.Description,
											Price = m.Price,
											ProductId = m.ProductItems.Product.ProductId,
											ProductName = m.ProductItems.Product.ProductName,
											Img = m.ProductItems.Image1,
											Ram = m.Ram.RamName,
											Ssd = m.Ssd.Ssdname,
											ProductVarId = m.ProductVarId
										});

				if (sortBy == 1) // Sắp xếp giảm dần
				{
					productQuery = productQuery.OrderByDescending(n => n.Price);
				}
				else if (sortBy == 0) // Sắp xếp tăng dần
				{
					productQuery = productQuery.OrderBy(n => n.Price);
				}

				var product = productQuery.ToList();

				return new JsonResult(product);
			}

		}
		[HttpGet]
		public IActionResult FilterByPrice(int minPrice, int maxPrice, int? categoryId, int? brandId)
		{

			if (categoryId == null)
			{
				var filteredProducts = _context.ProductVariations
						.Where(p => p.Price >= minPrice && p.Price <= maxPrice && p.ProductItems.Product.Brand == brandId)
						.Select(m => new
						{
							Brand = m.ProductItems.Product.Brand,
							Category = m.ProductItems.Product.Category.CategoryName,
							Description = m.ProductItems.Product.Description,
							Price = m.Price,
							ProductId = m.ProductItems.Product.ProductId,
							ProductName = m.ProductItems.Product.ProductName,
							Img = m.ProductItems.Image1,
							Ram = m.Ram.RamName,
							Ssd = m.Ssd.Ssdname,
							ProductVarId = m.ProductVarId
						}).ToList();

				return new JsonResult(filteredProducts.OrderBy(n => n.Price));
			}
			else if (brandId == null)
			{
				var filteredProducts = _context.ProductVariations
					.Where(p => p.Price >= minPrice && p.Price <= maxPrice && p.ProductItems.Product.CategoryId == categoryId)
											.Select(m => new
											{
												Brand = m.ProductItems.Product.Brand,
												Category = m.ProductItems.Product.Category.CategoryName,
												Description = m.ProductItems.Product.Description,
												Price = m.Price,
												ProductId = m.ProductItems.Product.ProductId,
												ProductName = m.ProductItems.Product.ProductName,
												Img = m.ProductItems.Image1,
												Ram = m.Ram.RamName,
												Ssd = m.Ssd.Ssdname,
												ProductVarId = m.ProductVarId
											}).ToList();

				return new JsonResult(filteredProducts.OrderBy(n => n.Price));
			}
			else
			{
				var filteredProducts = _context.ProductVariations
					.Where(p => p.Price >= minPrice && p.Price <= maxPrice)
					.Select(m => new
					{
						Brand = m.ProductItems.Product.Brand,
						BrandNavigation = m.ProductItems.Product.BrandNavigation,
						Category = m.ProductItems.Product.Category,
						CategoryId = categoryId,
						Description = m.ProductItems.Product.Description,
						Price = m.Price,
						ProductId = m.ProductItems.Product.ProductId,
						ProductName = m.ProductItems.Product.ProductName,
						ProductItems = m.ProductItems.Product.ProductItems.Select(pi => new
						{
							Image1 = pi.Image1,
							FirstProductVarId = pi.ProductVariations.Select(n => n.ProductVarId).FirstOrDefault() // Get only the first ProductVarId
						}).ToList(),
					}).ToList();

				return new JsonResult(filteredProducts.OrderBy(n => n.Price));
			}
		}
	

	}
}
