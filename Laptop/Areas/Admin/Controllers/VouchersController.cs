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
    public class VouchersController : Controller
    {
        private readonly LaptopContext _context;

        public VouchersController(LaptopContext context)
        {
            _context = context;
        }

        // GET: Admin/Vouchers
        public async Task<IActionResult> Index()
        {
              return _context.Vouchers != null ? 
                          View(await _context.Vouchers.ToListAsync()) :
                          Problem("Entity set 'LaptopContext.Vouchers'  is null.");
        }

        // GET: Admin/Vouchers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Vouchers == null)
            {
                return NotFound();
            }

            var voucher = await _context.Vouchers
                .FirstOrDefaultAsync(m => m.VoucherCode == id);
            if (voucher == null)
            {
                return NotFound();
            }

            return View(voucher);
        }

        // GET: Admin/Vouchers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Vouchers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VoucherCode,Discount,VoucherQuantity,StartDate,EndDate")] Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem VoucherCode đã tồn tại trong cơ sở dữ liệu hay không
                var existingVoucher = await _context.Vouchers.FirstOrDefaultAsync(v => v.VoucherCode == voucher.VoucherCode);

                if (existingVoucher != null)
                {
                    // Nếu VoucherCode đã tồn tại, hiển thị thông báo lỗi
                    ModelState.AddModelError("VoucherCode", "VoucherCode already exists.");
                    return View(voucher);
                }
                else
                {
                    // Nếu VoucherCode chưa tồn tại, thêm mới Voucher
                    _context.Add(voucher);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(voucher);
        }



        // GET: Admin/Vouchers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Vouchers == null)
            {
                return NotFound();
            }

            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher == null)
            {
                return NotFound();
            }
            return View(voucher);
        }


        // POST: Admin/Vouchers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("VoucherCode,Discount,VoucherQuantity,StartDate,EndDate")] Voucher voucher)
        {
            if (id != voucher.VoucherCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(voucher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoucherExists(voucher.VoucherCode))
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
            return View(voucher);
        }

        // GET: Admin/Vouchers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Vouchers == null)
            {
                return NotFound();
            }

            var voucher = await _context.Vouchers
                .FirstOrDefaultAsync(m => m.VoucherCode == id);
            if (voucher == null)
            {
                return NotFound();
            }

            return View(voucher);
        }

        // POST: Admin/Vouchers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Vouchers == null)
            {
                return Problem("Entity set 'LaptopContext.Vouchers'  is null.");
            }
            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher != null)
            {
                _context.Vouchers.Remove(voucher);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoucherExists(string id)
        {
          return (_context.Vouchers?.Any(e => e.VoucherCode == id)).GetValueOrDefault();
        }
    }
}
