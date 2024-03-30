using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laptop.Models;

namespace Laptop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductVariationsController : Controller
    {
        private readonly LaptopContext _context;

        public ProductVariationsController(LaptopContext context)
        {
            _context = context;
        }

        // GET: Admin/ProductVariations
        public async Task<IActionResult> Index()
        {
            var laptopContext = _context.ProductVariations.Include(p => p.ProductItems).Include(p => p.Ram).Include(p => p.Ssd);
            return View(await laptopContext.ToListAsync());
        }

        // GET: Admin/ProductVariations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductVariations == null)
            {
                return NotFound();
            }

            var productVariation = await _context.ProductVariations
                .Include(p => p.ProductItems)
                .Include(p => p.Ram)
                .Include(p => p.Ssd)
                .FirstOrDefaultAsync(m => m.ProductItemsId == id);
            if (productVariation == null)
            {
                return NotFound();
            }

            return View(productVariation);
        }

        // GET: Admin/ProductVariations/Create
        public IActionResult Create(int id)
        {
            ViewData["ProductItemsId"] = _context.ProductItems.Where(n => n.ProductItemsId == id).Include(n => n.Product).FirstOrDefault();
            var product = _context.ProductVariations.Where(n => n.ProductVarId == id)
                .Include(n => n.ProductItems)
                .FirstOrDefault();
            ViewData["RamId"] = new SelectList(_context.Rams, "RamId", "RamId");
            ViewData["Ssdid"] = new SelectList(_context.Ssds, "SsdId", "SsdId");
            return View(product);
        }

        // POST: Admin/ProductVariations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductVarId,ProductItemsId,RamId,Ssdid,QtyinStock,Price")] ProductVariation productVariation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productVariation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductItemsId"] = new SelectList(_context.ProductItems, "ProductItemsId", "ProductItemsId", productVariation.ProductItemsId);
            ViewData["RamId"] = new SelectList(_context.Rams, "RamId", "RamId", productVariation.RamId);
            ViewData["Ssdid"] = new SelectList(_context.Ssds, "SsdId", "SsdId", productVariation.Ssdid);
            return View(productVariation);
        }

        // GET: Admin/ProductVariations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductVariations == null)
            {
                return NotFound();
            }

            var productVariation = await _context.ProductVariations.FindAsync(id);
            if (productVariation == null)
            {
                return NotFound();
            }
            ViewData["ProductItemsId"] = new SelectList(_context.ProductItems, "ProductItemsId", "ProductItemsId", productVariation.ProductItemsId);
            ViewData["RamId"] = new SelectList(_context.Rams, "RamId", "RamId", productVariation.RamId);
            ViewData["Ssdid"] = new SelectList(_context.Ssds, "SsdId", "SsdId", productVariation.Ssdid);
            return View(productVariation);
        }

        // POST: Admin/ProductVariations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductVarId,ProductItemsId,RamId,Ssdid,QtyinStock,Price")] ProductVariation productVariation)
        {
            if (id != productVariation.ProductItemsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productVariation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductVariationExists(productVariation.ProductItemsId))
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
            ViewData["ProductItemsId"] = new SelectList(_context.ProductItems, "ProductItemsId", "ProductItemsId", productVariation.ProductItemsId);
            ViewData["RamId"] = new SelectList(_context.Rams, "RamId", "RamId", productVariation.RamId);
            ViewData["Ssdid"] = new SelectList(_context.Ssds, "SsdId", "SsdId", productVariation.Ssdid);
            return View(productVariation);
        }

        // GET: Admin/ProductVariations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductVariations == null)
            {
                return NotFound();
            }

            var productVariation = await _context.ProductVariations
                .Include(p => p.ProductItems)
                .Include(p => p.Ram)
                .Include(p => p.Ssd)
                .FirstOrDefaultAsync(m => m.ProductItemsId == id);
            if (productVariation == null)
            {
                return NotFound();
            }

            return View(productVariation);
        }

        // POST: Admin/ProductVariations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductVariations == null)
            {
                return Problem("Entity set 'LaptopContext.ProductVariations'  is null.");
            }
            var productVariation = await _context.ProductVariations.FindAsync(id);
            if (productVariation != null)
            {
                _context.ProductVariations.Remove(productVariation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductVariationExists(int id)
        {
          return (_context.ProductVariations?.Any(e => e.ProductItemsId == id)).GetValueOrDefault();
        }
    }
}
