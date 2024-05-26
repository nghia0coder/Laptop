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
            var laptopContext = _context.Warranties.Include(w => w.Customer).Include(w => w.Order);
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
                .Include(w => w.Customer)
                .Include(w => w.Order)
                .FirstOrDefaultAsync(m => m.WarrantyId == id);
            if (warranty == null)
            {
                return NotFound();
            }

            return View(warranty);
        }

        // GET: Admin/Warranties/Create
        public IActionResult Create()
        {
            // Truyền danh sách trạng thái vào ViewData
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
            //ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId");
            return View();
        }
        private async Task<int> GetCurrentUserAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return 0;
            }
            var customerId = _context.Customers.Where(n => n.AccountId == user.Id).Select(n => n.CustomerId).FirstOrDefault();

            if (customerId == 0)
            {
                customerId = _context.Employees.Where(n => n.AccountId == user.Id).Select(n => n.EmployeeId).FirstOrDefault();
            }
            return customerId;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WarrantyId,CustomerId,OrderId,CreateDate,EndDate,Reason,Service,Status,Note")] Warranty warranty)
        {

            warranty.CustomerId = await GetCurrentUserAsync();
            if (ModelState.IsValid)
            {
                _context.Add(warranty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Truyền lại danh sách trạng thái vào ViewData nếu ModelState không hợp lệ
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

            //ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", warranty.CustomerId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", warranty.OrderId);
            return View(warranty);
        }

        [HttpGet]
        public JsonResult GetProductVariationsByOrder(string orderId)
        {
            var productVariations = _context.OrdersDetails
                                             .Where(od => od.OrderId == orderId)
                                             .Include(od => od.ProductVar)
                                             .Select(od => new
                                             {
                                                 ProductVariationId = od.ProductVar.ProductVarId,
                                                 ProductName = od.ProductVar.ProductItems.Product.ProductName
                                             })
                                             .ToList();
            return Json(productVariations);
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
            var productVariations = await _context.OrdersDetails
                                                  .Where(od => od.OrderId == warranty.OrderId)
                                                  .Include(od => od.ProductVar)
                                                  .Select(od => new
                                                  {
                                                      ProductVariationId = od.ProductVar.ProductVarId,
                                                      //ProductName = od.ProductVar.ProductName // Assuming there's a ProductName property
                                                  })
                                                  .ToListAsync();

            ViewBag.ProductVariations = new SelectList(productVariations, "ProductVariationId", "ProductName");

            return View(warranty);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WarrantyId,CustomerId,OrderId,CreateDate,EndDate,Reason,Service,Status,Note")] Warranty warranty)
        {
            if (id != warranty.WarrantyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Ensure CustomerId is valid
                    var customerExists = await _context.Customers.AnyAsync(c => c.CustomerId == warranty.CustomerId);
                    if (!customerExists)
                    {
                        ModelState.AddModelError("CustomerId", "Customer does not exist.");
                        return await ReloadEditView(warranty);
                    }

                    _context.Update(warranty);
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

            return await ReloadEditView(warranty);
        }

        // Helper method to reload view data
        private async Task<IActionResult> ReloadEditView(Warranty warranty)
        {
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

            var productVariations = await _context.OrdersDetails
                                                  .Where(od => od.OrderId == warranty.OrderId)
                                                  .Include(od => od.ProductVar)
                                                  .Select(od => new
                                                  {
                                                      ProductVariationId = od.ProductVar.ProductVarId,
                                                      //ProductName = od.ProductVar.ProductName // Assuming there's a ProductName property
                                                  })
                                                  .ToListAsync();

            ViewBag.ProductVariations = new SelectList(productVariations, "ProductVariationId", "ProductName");

            return View(warranty);
        }


        // GET: Admin/Warranties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Warranties == null)
            {
                return NotFound();
            }

            var warranty = await _context.Warranties
                .Include(w => w.Customer)
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
