using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GiayDep.Models;
using Microsoft.AspNetCore.Authorization;

namespace GiayDep.Areas.Admin.Controllers
{   
    //Test
    [Area("Admin")]
    public class PhieuNhapController : Controller
    {
        private readonly LaptopContext _context;

        public PhieuNhapController(LaptopContext context)
        {
            _context = context;
        }

        // GET: Admin/PhieuNhap
        public async Task<IActionResult> Index()
        {
            return RedirectToAction("Index","CtPhieuNhaps");
        }

        // GET: Admin/PhieuNhap/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PhieuNhaps == null)
            {
                return NotFound();
            }

            var phieuNhap = await _context.PhieuNhaps
                .Include(p => p.IdnhaccNavigation)
                .FirstOrDefaultAsync(m => m.Idphieunhap == id);
            if (phieuNhap == null)
            {
                return NotFound();
            }

            return View(phieuNhap);
        }
        [Authorize(Roles = "Manager")]
        // GET: Admin/PhieuNhap/Create
        public IActionResult Create()
        {
            ViewBag.MaNCC = _context.NhaCungCaps.ToList();
            ViewBag.ListSanPham = _context.SanPhams.ToList();
            ViewBag.NgayNhap = DateTime.Today;
            return View();
        }
        [Authorize(Roles = "Manager")]
        // POST: Admin/PhieuNhap/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind("Idphieunhap,Ngaynhap,Idnhacc")] PhieuNhap model, IEnumerable<CtPhieuNhap> lstModel)
        {
            ViewBag.MaNCC = _context.NhaCungCaps.ToList();
            ViewBag.ListSanPham = _context.SanPhams.ToList();
            ViewBag.NgayNhap = DateTime.Now;
            model.Ngaynhap = ViewBag.NgayNhap;

            _context.PhieuNhaps.Add(model);
            _context.SaveChanges();
            SanPham sp;
            foreach (var item in lstModel)
            {          
                sp = _context.SanPhams.Single(n => n.Idsp == item.Idsp);
                sp.Soluong += item.Soluong;
                item.Idphieunhap = model.Idphieunhap;
                item.NgayNhap = model.Ngaynhap;
            }

            _context.CtPhieuNhaps.AddRange(lstModel);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        // GET: Admin/PhieuNhap/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PhieuNhaps == null)
            {
                return NotFound();
            }

            var phieuNhap = await _context.PhieuNhaps.FindAsync(id);
            if (phieuNhap == null)
            {
                return NotFound();
            }
            ViewData["Idnhacc"] = new SelectList(_context.NhaCungCaps, "Idnhacc", "Idnhacc", phieuNhap.Idnhacc);
            return View(phieuNhap);
        }

        // POST: Admin/PhieuNhap/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idphieunhap,Ngaynhap,Idnhacc")] PhieuNhap phieuNhap)
        {
            if (id != phieuNhap.Idphieunhap)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phieuNhap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhieuNhapExists(phieuNhap.Idphieunhap))
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
            ViewData["Idnhacc"] = new SelectList(_context.NhaCungCaps, "Idnhacc", "Idnhacc", phieuNhap.Idnhacc);
            return View(phieuNhap);
        }

        // GET: Admin/PhieuNhap/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PhieuNhaps == null)
            {
                return NotFound();
            }

            var phieuNhap = await _context.PhieuNhaps
                .Include(p => p.IdnhaccNavigation)
                .FirstOrDefaultAsync(m => m.Idphieunhap == id);
            if (phieuNhap == null)
            {
                return NotFound();
            }

            return View(phieuNhap);
        }

        // POST: Admin/PhieuNhap/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PhieuNhaps == null)
            {
                return Problem("Entity set 'LaptopContext.PhieuNhaps'  is null.");
            }
            var phieuNhap = await _context.PhieuNhaps.FindAsync(id);
            if (phieuNhap != null)
            {
                _context.PhieuNhaps.Remove(phieuNhap);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        public ActionResult DSSPHetHang()
        {
            var lstSP = _context.SanPhams.Where(n => n.Soluong <= 5).ToList();
            return View(lstSP);
        }

        [HttpGet]
        public ActionResult NhapHangDon(int? id)
        {
            ViewBag.MaNCC = new SelectList(_context.NhaCungCaps.OrderBy(n => n.Tennhacc), "Idnhacc", "Tennhacc");

            if (id == null)
            {
                return NotFound();
            }

            SanPham sp = _context.SanPhams.SingleOrDefault(n => n.Idsp == id);
            if (sp == null)
            {
                return NotFound();
            }

            return View(sp);
        }

        [HttpPost]
        public ActionResult NhapHangDon(PhieuNhap model, CtPhieuNhap ctpn)
        {
            ViewBag.MaNCC = new SelectList(_context.NhaCungCaps.OrderBy(n => n.Tennhacc), "Idnhacc", "Tennhacc", model.Idnhacc);

            model.Ngaynhap = DateTime.Now;

            _context.PhieuNhaps.Add(model);
            _context.SaveChanges();

            ctpn.Idphieunhap = model.Idphieunhap;
            SanPham sp = _context.SanPhams.Single(n => n.Idsp == ctpn.Idsp);
            sp.Soluong += ctpn.Soluong;

            _context.CtPhieuNhaps.Add(ctpn);
            _context.SaveChanges();

            return RedirectToAction("Index","Products");
        }

        private bool PhieuNhapExists(int id)
        {
          return (_context.PhieuNhaps?.Any(e => e.Idphieunhap == id)).GetValueOrDefault();
        }
    }
}
