using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laptop.Models;
using Microsoft.AspNetCore.Hosting;
using Laptop.Models;


namespace Laptop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly LaptopContext _context;
        private readonly IWebHostEnvironment _webHost;

        public ProductsController(LaptopContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }
        // GET: Admin/Products
        public async Task<IActionResult> Index(/*int pg=1*/)
        {
            const int pageSize = 5;

            var laptopContext = _context.Products
                .Include(n => n.Category)
                .Include(n => n.BrandNavigation)
                .ToList(); // Retrieve all products

            // Perform pagination logic
            //if (pg < 1)
            //    pg = 1;

            //int recsCount = laptopContext.Count();

            //var pager = new Pager(recsCount, pg, pageSize);

            //int recSkip = (pg - 1) * pageSize;

            //var data = laptopContext.Skip(recSkip).Take(pageSize).ToList();

            //ViewBag.Pager = pager;

            return View(laptopContext); // Pass the paged data to the view
        }
        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var Product = await _context.Products
                .Include(_ => _.Category)
                .Include(_ => _.BrandNavigation)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId");
            ViewData["Brand"] = new SelectList(_context.Brands, "BrandId", "BrandName");
            Product Product = new Product()
            {
                New = false
            };
            return View(Product);
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem các trường không được để trống
                if (string.IsNullOrEmpty(product.ProductName) || product.CategoryId == null || product.Brand == null)
                {
                    ModelState.AddModelError("", "All fields are required.");
                    ViewBag.Brands = new SelectList(_context.Brands, "BrandId", "BrandName", product.Brand);
                    ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
                    return View(product);
                }
                // Kiểm tra xem tên sản phẩm đã tồn tại trong cơ sở dữ liệu chưa
                var existingProduct = _context.Products.FirstOrDefault(p => p.ProductName == product.ProductName);

                if (existingProduct != null)
                {
                    // Nếu tên sản phẩm đã tồn tại, hiển thị thông báo lỗi
                    ModelState.AddModelError("ProductName", "Product name already exists.");
                    // Load lại các danh sách thương hiệu và loại sản phẩm
                    ViewBag.Brands = new SelectList(_context.Brands, "BrandId", "BrandName", product.Brand);
                    ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
                    return View(product);
                }

                // Nếu không có lỗi, thêm sản phẩm mới vào cơ sở dữ liệu
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            // Nếu ModelState không hợp lệ, load lại các danh sách thương hiệu và loại sản phẩm
            ViewBag.Brands = new SelectList(_context.Brands, "BrandId", "BrandName", product.Brand);
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);

            return View(product);
        }



        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var Product = await _context.Products.FindAsync(id);
            if (Product == null)
            {
                return NotFound();
            }

            //TempData["img1"] = Product.Hinhanh1;
            //TempData["img2"] = Product.Hinhanh2;
            //TempData["img3"] = Product.Hinhanh3;
            //TempData["img4"] = Product.Hinhanh4;

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", Product.CategoryId);
            ViewData["Brand"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierId", Product.Brand);
	
		
			return View(Product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Kiểm tra xem các trường không được để trống
                if (string.IsNullOrEmpty(product.ProductName))
                {
                    ModelState.AddModelError("", "All fields are required.");
                    // Load lại danh sách thương hiệu và loại sản phẩm
                    ViewBag.Brands = new SelectList(_context.Brands, "BrandId", "BrandName", product.Brand);
                    ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
                    return View(product);
                }

                // Kiểm tra xem ProductName không được trùng
                var existingProduct = _context.Products.FirstOrDefault(p => p.ProductName == product.ProductName && p.ProductId != product.ProductId);

                if (existingProduct != null)
                {
                    ModelState.AddModelError("ProductName", "Product name already exists.");
                    // Load lại danh sách thương hiệu và loại sản phẩm
                    ViewBag.Brands = new SelectList(_context.Brands, "BrandId", "BrandName", product.Brand);
                    ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
                    return View(product);
                }

                try
                {
                    _context.Update(product);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Load lại danh sách thương hiệu và loại sản phẩm
            ViewBag.Brands = new SelectList(_context.Brands, "BrandId", "BrandName", product.Brand);
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);

            return View(product);
        }



        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var Product = await _context.Products
         
                .Include(s => s.BrandNavigation)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'LaptopContext.Products'  is null.");
            }
            var Product = await _context.Products.FindAsync(id);
            if (Product != null)
            {
                _context.Products.Remove(Product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
       


        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
