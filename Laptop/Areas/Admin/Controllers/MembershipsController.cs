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
    public class MembershipsController : Controller
    {
        private readonly LaptopContext _context;

        public MembershipsController(LaptopContext context)
        {
            _context = context;
        }

        // GET: Admin/Memberships
        public async Task<IActionResult> Index()
        {
            var LaptopContext = _context.Memberships.Include(m => m.MaLoaiTvNavigation);
            return View(await LaptopContext.ToListAsync());
        }

        // GET: Admin/Memberships/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Memberships == null)
            {
                return NotFound();
            }

            var membership = await _context.Memberships
                .Include(m => m.MaLoaiTvNavigation)
                .FirstOrDefaultAsync(m => m.Idtv == id);
            if (membership == null)
            {
                return NotFound();
            }

            return View(membership);
        }

        // GET: Admin/Memberships/Create
        public IActionResult Create()
        {
            ViewData["MaLoaiTv"] = new SelectList(_context.MembershipTypes, "MaLoaiTv", "MaLoaiTv");
            return View();
        }

        // POST: Admin/Memberships/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idtv,Tk,Mk,HoTen,DiaChi,Email,Sđt,MaLoaiTv")] Membership membership)
        {
            if (ModelState.IsValid)
            {
                _context.Add(membership);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaLoaiTv"] = new SelectList(_context.MembershipTypes, "MaLoaiTv", "MaLoaiTv", membership.MaLoaiTv);
            return View(membership);
        }

        // GET: Admin/Memberships/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Memberships == null)
            {
                return NotFound();
            }

            var membership = await _context.Memberships.FindAsync(id);
            if (membership == null)
            {
                return NotFound();
            }
            ViewData["MaLoaiTv"] = new SelectList(_context.MembershipTypes, "MaLoaiTv", "MaLoaiTv", membership.MaLoaiTv);
            return View(membership);
        }

        // POST: Admin/Memberships/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idtv,Tk,Mk,HoTen,DiaChi,Email,Sđt,MaLoaiTv")] Membership membership)
        {
            if (id != membership.Idtv)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(membership);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MembershipExists(membership.Idtv))
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
            ViewData["MaLoaiTv"] = new SelectList(_context.MembershipTypes, "MaLoaiTv", "MaLoaiTv", membership.MaLoaiTv);
            return View(membership);
        }

        // GET: Admin/Memberships/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Memberships == null)
            {
                return NotFound();
            }

            var membership = await _context.Memberships
                .Include(m => m.MaLoaiTvNavigation)
                .FirstOrDefaultAsync(m => m.Idtv == id);
            if (membership == null)
            {
                return NotFound();
            }

            return View(membership);
        }

        // POST: Admin/Memberships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Memberships == null)
            {
                return Problem("Entity set 'LaptopContext.Memberships'  is null.");
            }
            var membership = await _context.Memberships.FindAsync(id);
            if (membership != null)
            {
                _context.Memberships.Remove(membership);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MembershipExists(int id)
        {
          return (_context.Memberships?.Any(e => e.Idtv == id)).GetValueOrDefault();
        }
    }
}
