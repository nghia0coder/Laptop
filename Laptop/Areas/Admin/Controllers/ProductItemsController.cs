using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laptop.Models;
using Microsoft.AspNetCore;
using Laptop.ViewModels;

namespace Laptop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductItemsController : Controller
    {
        private readonly LaptopContext _context;
        private readonly IWebHostEnvironment _webHost;

        public ProductItemsController(LaptopContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }

        // GET: Admin/ProductItems
        public async Task<IActionResult> Index(int pg = 1)
        {
            const int pageSize = 5;

            var productItems = await _context.ProductItems
                .Include(p => p.Color)
                .Include(p => p.Product)
                .OrderBy(p => p.ProductItemsId)
                
                .ToListAsync();

            //var totalProductItems = await _context.ProductItems.CountAsync();
            //var pager = new Pager(totalProductItems, pg, pageSize);

            //ViewBag.Pager = pager;

            return View(productItems);
        }

        // GET: Admin/ProductItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductItems == null)
            {
                return NotFound();
            }

            var productItem = await _context.ProductItems
                .Include(p => p.Color)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductItemsId == id);

            ViewData["product"] = await _context.ProductVariations
                .Where(n => n.ProductItemsId == productItem.ProductItemsId)
                .Include(n => n.Ram)
                .Include(n => n.Ssd)
                .ToListAsync();
            

            if (productItem == null)
            {
                return NotFound();
            }

            return View(productItem);
        }

        // GET: Admin/ProductItems/Create
        public IActionResult Create(int? id)
        {
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName");
            var product = _context.Products.Find(id);
            ViewData["ProductName"] = product.ProductName;
            ViewData["ProductId"] = id;
            ViewData["Ram"] = new SelectList(_context.Rams, "RamId", "RamName");
            ViewData["SSD"] = new SelectList(_context.Ssds, "SsdId", "Ssdname");
            return View();
        }

        // POST: Admin/ProductItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductItemViewModel product)
        {

            string uniqueFileName1 = GetProfilePhotoFileName1(product);
            string uniqueFileName2 = GetProfilePhotoFileName2(product);
            string uniqueFileName3 = GetProfilePhotoFileName3(product);
            string uniqueFileName4 = GetProfilePhotoFileName4(product);
     
            var productItem = new ProductItem()
            {
                ProductId = product.ProductId,
                ProductCode = product.ProductCode,
                ColorId = product.ColorId,
                Image1 = uniqueFileName1,
                Image2 = uniqueFileName2,
                Image3 = uniqueFileName3,
                Image4 = uniqueFileName4

            };
            _context.ProductItems.Add(productItem);
            await _context.SaveChangesAsync();

       

          
            var productVar = new ProductVariation()
                {
                   ProductItemsId = productItem.ProductItemsId,
                   RamId = product.Ram,
                   Ssdid = product.Ssd,
                   QtyinStock = 0
                };
                _context.ProductVariations.Add(productVar);
         

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/ProductItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductItems == null)
            {
                return NotFound();
            }

            var productItem = await _context.ProductItems.Where(n=>n.ProductItemsId == id).FirstOrDefaultAsync();
            if (productItem == null)
            {
                return NotFound();
            }
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorId", productItem.ColorId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", productItem.ProductId);
            return View(productItem);
        }

        // POST: Admin/ProductItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductItem productItem)
        {
            if (id != productItem.ProductItemsId)
            {
                return NotFound();
            }
            string uniqueFileName1 = GetProfilePhotoFileName1(productItem);
            string uniqueFileName2 = GetProfilePhotoFileName2(productItem);
            string uniqueFileName3 = GetProfilePhotoFileName3(productItem);
            string uniqueFileName4 = GetProfilePhotoFileName4(productItem);
            productItem.Image1 = uniqueFileName1;
            productItem.Image2 = uniqueFileName2;
            productItem.Image3 = uniqueFileName3;
            productItem.Image4 = uniqueFileName4;


             _context.Update(productItem);
             await _context.SaveChangesAsync();
             
                return RedirectToAction(nameof(Index));
            
     
            return View(productItem);
        }

        // GET: Admin/ProductItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductItems == null)
            {
                return NotFound();
            }

            var productItem = await _context.ProductItems
                .Include(p => p.Color)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductItemsId == id);
            if (productItem == null)
            {
                return NotFound();
            }

            return View(productItem);
        }

        // POST: Admin/ProductItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductItems == null)
            {
                return Problem("Entity set 'LaptopContext.ProductItems'  is null.");
            }
            var productItem = await _context.ProductItems.Where(n => n.ProductItemsId == id).FirstOrDefaultAsync();
            if (productItem != null)
            {
                _context.ProductItems.Remove(productItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductItemExists(int id)
        {
          return (_context.ProductItems?.Any(e => e.ProductItemsId == id)).GetValueOrDefault();
        }
        private string GetProfilePhotoFileName1(ProductItemViewModel Product)
        {
            string uniqueFileName = null;

            if (Product.Img1 != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "Contents/img/");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Product.Img1.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Product.Img1.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        private string GetProfilePhotoFileName2(ProductItemViewModel Product)
        {
            string uniqueFileName = null;

            if (Product.Img2 != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "Contents/img/");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Product.Img2.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Product.Img2.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        private string GetProfilePhotoFileName3(ProductItemViewModel Product)
        {
            string uniqueFileName = null;

            if (Product.Img3 != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "Contents/img/");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Product.Img3.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Product.Img3.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        private string GetProfilePhotoFileName4(ProductItemViewModel Product)
        {
            string uniqueFileName = null;

            if (Product.Img4 != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "Contents/img/");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Product.Img4.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Product.Img4.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        private string GetProfilePhotoFileName1(ProductItem Product)
        {
            string uniqueFileName = null;

            if (Product.Img1 != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "Contents/img/");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Product.Img1.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Product.Img1.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        private string GetProfilePhotoFileName2(ProductItem Product)
        {
            string uniqueFileName = null;

            if (Product.Img2 != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "Contents/img/");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Product.Img2.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Product.Img2.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        private string GetProfilePhotoFileName3(ProductItem Product)
        {
            string uniqueFileName = null;

            if (Product.Img3 != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "Contents/img/");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Product.Img3.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Product.Img3.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        private string GetProfilePhotoFileName4(ProductItem Product)
        {
            string uniqueFileName = null;

            if (Product.Img4 != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "Contents/img/");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Product.Img4.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Product.Img4.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
