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
    public class SuppliersController : Controller
    {
        private readonly LaptopContext _context;

        public SuppliersController(LaptopContext context)
        {
            _context = context;
        }

        // GET: Admin/Suppliers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Suppliers.ToListAsync());
        }

        // GET: Admin/Suppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
           
            var suppiler = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.SupplierId == id);
            if (suppiler == null)
            {
                return NotFound();
            }

            return View(suppiler);
        }

        // GET: Admin/Suppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Suppliers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SupplierId,SupplierName,Address,Phone,Email")] Supplier suppiler)
        {
            if (ModelState.IsValid)
            {
                _context.Add(suppiler);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(suppiler);
        }

        // GET: Admin/Suppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Suppliers == null)
            {
                return NotFound();
            }

            var suppiler = await _context.Suppliers.FindAsync(id);
            if (suppiler == null)
            {
                return NotFound();
            }
            return View(suppiler);
        }

        // POST: Admin/Suppliers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SupplierId,SupplierName,Address,Phone,Email")] Supplier suppiler)
        {
            if (id != suppiler.SupplierId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suppiler);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuppilerExists(suppiler.SupplierId))
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
            return View(suppiler);
        }

        // GET: Admin/Suppliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Suppliers == null)
            {
                return NotFound();
            }

            var suppiler = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.SupplierId == id);
            if (suppiler == null)
            {
                return NotFound();
            }

            return View(suppiler);
        }

        // POST: Admin/Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Suppliers == null)
            {
                return Problem("Entity set 'LaptopContext.Suppliers'  is null.");
            }
            var suppiler = await _context.Suppliers.FindAsync(id);
            if (suppiler != null)
            {
                _context.Suppliers.Remove(suppiler);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuppilerExists(int id)
        {
          return (_context.Suppliers?.Any(e => e.SupplierId == id)).GetValueOrDefault();
        }
    }
}
