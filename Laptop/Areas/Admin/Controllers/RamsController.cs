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
    public class RamsController : Controller
    {
        private readonly LaptopContext _context;

        public RamsController(LaptopContext context)
        {
            _context = context;
        }

        // GET: Admin/Rams
        public async Task<IActionResult> Index()
        {
              return _context.Rams != null ? 
                          View(await _context.Rams.ToListAsync()) :
                          Problem("Entity set 'LaptopContext.Rams'  is null.");
        }

        // GET: Admin/Rams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Rams == null)
            {
                return NotFound();
            }

            var ram = await _context.Rams
                .FirstOrDefaultAsync(m => m.RamId == id);
            if (ram == null)
            {
                return NotFound();
            }

            return View(ram);
        }

        // GET: Admin/Rams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Rams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RamId,RamName")] Ram ram)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem RamName đã được cung cấp hay không
                if (string.IsNullOrEmpty(ram.RamName))
                {
                    ModelState.AddModelError("RamName", "RAM name is required.");
                    return View(ram);
                }

                // Kiểm tra xem RamName đã tồn tại trong cơ sở dữ liệu chưa
                var existingRam = await _context.Rams.FirstOrDefaultAsync(r => r.RamName == ram.RamName);

                if (existingRam != null)
                {
                    // Nếu RamName đã tồn tại, hiển thị thông báo lỗi
                    ModelState.AddModelError("RamName", "RAM already exists.");
                    return View(ram);
                }

                // Nếu RamName không tồn tại, tiếp tục thêm vào cơ sở dữ liệu
                _context.Add(ram);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ram);
        }


        // GET: Admin/Rams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Rams == null)
            {
                return NotFound();
            }

            var ram = await _context.Rams.FindAsync(id);
            if (ram == null)
            {
                return NotFound();
            }
            return View(ram);
        }

        // POST: Admin/Rams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RamId,RamName")] Ram ram)
        {
            if (id != ram.RamId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Kiểm tra xem RamName đã được cung cấp hay không
                if (string.IsNullOrEmpty(ram.RamName))
                {
                    ModelState.AddModelError("RamName", "RAM name is required.");
                    return View(ram);
                }

                // Kiểm tra xem RamName đã tồn tại trong cơ sở dữ liệu chưa
                var existingRam = await _context.Rams.FirstOrDefaultAsync(r => r.RamName == ram.RamName && r.RamId != ram.RamId);

                if (existingRam != null)
                {
                    // Nếu RamName đã tồn tại, hiển thị thông báo lỗi
                    ModelState.AddModelError("RamName", "RAM already exists.");
                    return View(ram);
                }

                try
                {
                    _context.Update(ram);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RamExists(ram.RamId))
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
            return View(ram);
        }

        // GET: Admin/Rams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Rams == null)
            {
                return NotFound();
            }

            var ram = await _context.Rams
                .FirstOrDefaultAsync(m => m.RamId == id);
            if (ram == null)
            {
                return NotFound();
            }

            return View(ram);
        }

        // POST: Admin/Rams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Rams == null)
            {
                return Problem("Entity set 'LaptopContext.Rams'  is null.");
            }
            var ram = await _context.Rams.FindAsync(id);
            if (ram != null)
            {
                _context.Rams.Remove(ram);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RamExists(int id)
        {
          return (_context.Rams?.Any(e => e.RamId == id)).GetValueOrDefault();
        }
    }
}
