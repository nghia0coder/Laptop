using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laptop.Models;
using Microsoft.AspNetCore.Identity;

namespace Laptop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WarrantiesController : Controller
    {
        private readonly LaptopContext _context;
        public readonly UserManager<AppUserModel> _userManager;

        public WarrantiesController(LaptopContext context, UserManager<AppUserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/Warranties
        public async Task<IActionResult> Index()
        {
            var laptopContext = _context.Warranties.Include(w => w.Order);
            return View(await laptopContext.ToListAsync());
        }

        // GET: Admin/Warranties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Warranties == null)
            {
                return NotFound();
            }

            var warranty = await _context.Warranties
                .Include(w => w.Order)
                .FirstOrDefaultAsync(m => m.WarrantyId == id);
            if (warranty == null)
            {
                return NotFound();
            }
            ViewBag.Product=_context.ProductVariations
                                    .Include(n => n.ProductItems)
                                    .ThenInclude(n => n.Product)
                                    .Where(n => n.ProductVarId == warranty.ProductId).FirstOrDefault();

            return View(warranty);
        }

        // GET: Admin/Warranties/Create
        public IActionResult Create()
        {
            ViewData["StatusList"] = new SelectList(new[]
            {
                new { Value = "Đang bảo hành", Text = "Đang bảo hành" },
                new { Value = "Đã hoàn thành", Text = "Đã hoàn thành" }
            }, "Value", "Text");

            ViewData["ServiceList"] = new SelectList(new[]
            {
                new { Value = "Kiểm tra, bảo dưỡng máy", Text = "Kiểm tra, bảo dưỡng máy" },
                new { Value = "Vệ sinh phần cứng", Text = "Vệ sinh phần cứng" },
                new { Value = "Cài đặt lại thiết bị", Text = "Cài đặt lại thiết bị" }
            }, "Value", "Text");


            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string? id, [Bind("WarrantyId,OrderId,CreateDate,EndDate,Reason,Service,Status,Note,ProductId")] Warranty warranty)
        {
            if (ModelState.IsValid)
            {
                _context.Add(warranty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["StatusList"] = new SelectList(new[]
            {
                new { Value = "Đang bảo hành", Text = "Đang bảo hành" },
                new { Value = "Đã hoàn thành", Text = "Đã hoàn thành" }
            }, "Value", "Text");

            ViewData["ServiceList"] = new SelectList(new[]
            {
                new { Value = "Kiểm tra, bảo dưỡng máy", Text = "Kiểm tra, bảo dưỡng máy" },
                new { Value = "Vệ sinh phần cứng", Text = "Vệ sinh phần cứng" },
                new { Value = "Cài đặt lại thiết bị", Text = "Cài đặt lại thiết bị" }
            }, "Value", "Text");


            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", warranty.OrderId);
            return View(warranty);
        }
        public IActionResult SearchOrders(string query)
        {
            var orders = _context.Orders
                .Where(o => o.OrderId.Contains(query))
                .Select(o => new { orderId = o.OrderId })
                .ToList();

            return Json(orders);
        }

        public IActionResult GetProductVariationsByOrder(string orderId)
        {
            var productList = _context.OrdersDetails
                .Where(od => od.OrderId == orderId)
                .Select(od => new { od.ProductVar.ProductVarId, od.ProductVar.ProductItems.Product.ProductName })
                .ToList();

            return Json(productList);
        }



        // GET: Admin/Warranties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Warranties == null)
            {
                return NotFound();
            }

            var warranty = await _context.Warranties.FindAsync(id);
            if (warranty == null)
            {
                return NotFound();
            }

            //ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", warranty.CustomerId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", warranty.OrderId);

            ViewData["StatusList"] = new SelectList(new[]
            {
                new { Value = "Đang bảo hành", Text = "Đang bảo hành" },
                new { Value = "Đã hoàn thành", Text = "Đã hoàn thành" }
            }, "Value", "Text");

            ViewData["ServiceList"] = new SelectList(new[]
            {
                new { Value = "Kiểm tra, bảo dưỡng máy", Text = "Kiểm tra, bảo dưỡng máy" },
                new { Value = "Vệ sinh phần cứng", Text = "Vệ sinh phần cứng" },
                new { Value = "Cài đặt lại thiết bị", Text = "Cài đặt lại thiết bị" }
            }, "Value", "Text");

            // Load ProductVariations for the initial OrderId
            var productVariations = _context.OrdersDetails
                                             .Where(od => od.OrderId == warranty.OrderId)
                                             .Include(od => od.ProductVar)
                                             .Select(od => new
                                             {
                                                 ProductVariationId = od.ProductVar.ProductVarId,
                                             })
                                             .ToList();

            ViewBag.ProductVariations = new SelectList(productVariations, "ProductVariationId", "ProductName");

            return View(warranty);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Warranty warranty)
        {
            if (id != warranty.WarrantyId)
            {
                return NotFound();
            }
            var warranty1 = _context.Warranties.Where(n => n.WarrantyId == id).FirstOrDefault();
            warranty1.Status = warranty.Status;
           try
                {   
                    _context.Update(warranty1);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WarrantyExists(warranty.WarrantyId))
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


        // GET: Admin/Warranties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Warranties == null)
            {
                return NotFound();
            }

            var warranty = await _context.Warranties
                .Include(w => w.Order)
                .FirstOrDefaultAsync(m => m.WarrantyId == id);
            if (warranty == null)
            {
                return NotFound();
            }

            return View(warranty);
        }

        // POST: Admin/Warranties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Warranties == null)
            {
                return Problem("Entity set 'LaptopContext.Warranties'  is null.");
            }
            var warranty = await _context.Warranties.FindAsync(id);
            if (warranty != null)
            {
                _context.Warranties.Remove(warranty);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WarrantyExists(int id)
        {
            return (_context.Warranties?.Any(e => e.WarrantyId == id)).GetValueOrDefault();
        }
    }
}
