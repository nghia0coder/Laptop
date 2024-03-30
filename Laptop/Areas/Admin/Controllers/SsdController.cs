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
    public class SsdController : Controller
    {
        private readonly LaptopContext _context;

        public SsdController(LaptopContext context)
        {
            _context = context;
        }

        // GET: Admin/Ssd
        public async Task<IActionResult> Index()
        {
            return _context.Ssds != null ?
                        View(await _context.Ssds.ToListAsync()) :
                        Problem("Entity set 'LaptopContext.Ssds'  is null.");
        }

        // GET: Admin/Ssd/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ssds == null)
            {
                return NotFound();
            }

            var ssd = await _context.Ssds
                .FirstOrDefaultAsync(m => m.SsdId == id);
            if (ssd == null)
            {
                return NotFound();
            }

            return View(ssd);
        }

        // GET: Admin/Ssd/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Ssd/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SsdId,Ssdname")] Ssd ssd)
        {
            // Kiểm tra xem SsdName đã được cung cấp hay không
            if (string.IsNullOrEmpty(ssd.Ssdname))
            {
                ModelState.AddModelError("Ssdname", "SSD name is required.");
                return View(ssd);
            }

            // Kiểm tra xem SsdName đã tồn tại trong cơ sở dữ liệu chưa
            var existingSsd = await _context.Ssds.FirstOrDefaultAsync(s => s.Ssdname == ssd.Ssdname);

            if (existingSsd != null)
            {
                // Nếu SsdName đã tồn tại, hiển thị thông báo lỗi
                ModelState.AddModelError("Ssdname", "SSD already exists.");
                return View(ssd);
            }

            if (ModelState.IsValid)
            {
                _context.Add(ssd);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ssd);
        }


        // GET: Admin/Ssd/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ssds == null)
            {
                return NotFound();
            }

            var ssd = await _context.Ssds.FindAsync(id);
            if (ssd == null)
            {
                return NotFound();
            }
            return View(ssd);
        }

        // POST: Admin/Ssd/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SsdId,Ssdname")] Ssd ssd)
        {
            if (id != ssd.SsdId)
            {
                return NotFound();
            }

            // Kiểm tra xem SsdName đã được cung cấp hay không
            if (string.IsNullOrEmpty(ssd.Ssdname))
            {
                ModelState.AddModelError("Ssdname", "SSD name is required.");
                return View(ssd);
            }

            // Kiểm tra xem SsdName đã tồn tại trong cơ sở dữ liệu chưa
            var existingSsd = await _context.Ssds.FirstOrDefaultAsync(s => s.Ssdname == ssd.Ssdname && s.SsdId != id);

            if (existingSsd != null)
            {
                // Nếu SsdName đã tồn tại, hiển thị thông báo lỗi
                ModelState.AddModelError("Ssdname", "SSD already exists.");
                return View(ssd);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ssd);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SsdExists(ssd.SsdId))
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
            return View(ssd);
        }


        // GET: Admin/Ssd/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ssds == null)
            {
                return NotFound();
            }

            var ssd = await _context.Ssds
                .FirstOrDefaultAsync(m => m.SsdId == id);
            if (ssd == null)
            {
                return NotFound();
            }

            return View(ssd);
        }

        // POST: Admin/Ssd/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ssds == null)
            {
                return Problem("Entity set 'LaptopContext.Ssds'  is null.");
            }
            var ssd = await _context.Ssds.FindAsync(id);
            if (ssd != null)
            {
                _context.Ssds.Remove(ssd);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SsdExists(int id)
        {
            return (_context.Ssds?.Any(e => e.SsdId == id)).GetValueOrDefault();
        }
    }
}
