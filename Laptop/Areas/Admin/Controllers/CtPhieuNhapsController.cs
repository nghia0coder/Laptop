using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GiayDep.Models;

namespace GiayDep.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CtPhieuNhapsController : Controller
    {
        private readonly LaptopContext _context;

        public CtPhieuNhapsController(LaptopContext context)
        {
            _context = context;
        }

        // GET: Admin/CtPhieuNhaps
        public async Task<IActionResult> Index()
        {
            var LaptopContext = _context.CtPhieuNhaps.Include(c => c.IdphieunhapNavigation).Include(c => c.IdspNavigation);
            return View(await LaptopContext.ToListAsync());
        }

        // GET: Admin/CtPhieuNhaps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CtPhieuNhaps == null)
            {
                return NotFound();
            }

            var ctPhieuNhap = await _context.CtPhieuNhaps
                .Include(c => c.IdphieunhapNavigation)
                .Include(c => c.IdspNavigation)
                .FirstOrDefaultAsync(m => m.IdchitietPn == id);
            if (ctPhieuNhap == null)
            {
                return NotFound();
            }

            return View(ctPhieuNhap);
        }

        // GET: Admin/CtPhieuNhaps/Create
        public IActionResult Create()
        {
            ViewData["Idphieunhap"] = new SelectList(_context.PhieuNhaps, "Idphieunhap", "Idphieunhap");
            ViewData["Idsp"] = new SelectList(_context.SanPhams, "Idsp", "Idsp");
            return View();
        }

        // POST: Admin/CtPhieuNhaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdchitietPn,Idsp,Idphieunhap,Soluong,Gia")] CtPhieuNhap ctPhieuNhap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ctPhieuNhap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idphieunhap"] = new SelectList(_context.PhieuNhaps, "Idphieunhap", "Idphieunhap", ctPhieuNhap.Idphieunhap);
            ViewData["Idsp"] = new SelectList(_context.SanPhams, "Idsp", "Idsp", ctPhieuNhap.Idsp);
            return View(ctPhieuNhap);
        }

        // GET: Admin/CtPhieuNhaps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CtPhieuNhaps == null)
            {
                return NotFound();
            }

            var ctPhieuNhap = await _context.CtPhieuNhaps.FindAsync(id);
            if (ctPhieuNhap == null)
            {
                return NotFound();
            }
            ViewData["Idphieunhap"] = new SelectList(_context.PhieuNhaps, "Idphieunhap", "Idphieunhap", ctPhieuNhap.Idphieunhap);
            ViewData["Idsp"] = new SelectList(_context.SanPhams, "Idsp", "Idsp", ctPhieuNhap.Idsp);
            return View(ctPhieuNhap);
        }

        // POST: Admin/CtPhieuNhaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdchitietPn,Idsp,Idphieunhap,Soluong,Gia")] CtPhieuNhap ctPhieuNhap)
        {
            if (id != ctPhieuNhap.IdchitietPn)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ctPhieuNhap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CtPhieuNhapExists(ctPhieuNhap.IdchitietPn))
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
            ViewData["Idphieunhap"] = new SelectList(_context.PhieuNhaps, "Idphieunhap", "Idphieunhap", ctPhieuNhap.Idphieunhap);
            ViewData["Idsp"] = new SelectList(_context.SanPhams, "Idsp", "Idsp", ctPhieuNhap.Idsp);
            return View(ctPhieuNhap);
        }

        // GET: Admin/CtPhieuNhaps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CtPhieuNhaps == null)
            {
                return NotFound();
            }
            var ctPhieuNhap = await _context.CtPhieuNhaps
                .Include(c => c.IdphieunhapNavigation)
                .Include(c => c.IdspNavigation)
                .FirstOrDefaultAsync(m => m.IdchitietPn == id);
            if (ctPhieuNhap == null)
            {
                return NotFound();
            }
            return View(ctPhieuNhap);
        }

        // POST: Admin/CtPhieuNhaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CtPhieuNhaps == null)
            {
                return Problem("Entity set 'LaptopContext.CtPhieuNhaps'  is null.");
            }
            var ctPhieuNhap = await _context.CtPhieuNhaps.FindAsync(id);
            if (ctPhieuNhap != null)
            {
                _context.CtPhieuNhaps.Remove(ctPhieuNhap);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CtPhieuNhapExists(int id)
        {
          return (_context.CtPhieuNhaps?.Any(e => e.IdchitietPn == id)).GetValueOrDefault();
        }
    }
}
