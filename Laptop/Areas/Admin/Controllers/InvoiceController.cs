using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laptop.Models;
using Microsoft.AspNetCore.Authorization;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using Laptop.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Laptop.Areas.Admin.Controllers
{   
    
    [Area("Admin")]
    public class InvoiceController : Controller
    {
        private readonly LaptopContext _context;
        private readonly UserManager<AppUserModel> _userManager;
        public InvoiceController(LaptopContext context, UserManager<AppUserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/Invoice
        public async Task<IActionResult> Index()
        {   
            var details = _context.InvoiceDetails
                .Include(n => n.ProductVar.ProductItems.Product)
                .Include(n => n.Invoice)
                .Include(n => n.ProductVar.ProductItems.Color)
                .Include(n => n.ProductVar.Ram)
                 .Include(n => n.ProductVar.Ssd)
                .ToList();
            return View(details);
        }

        // GET: Admin/Invoice/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var Invoice = await _context.Invoices
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.InvoiceId == id);
            if (Invoice == null)
            {
                return NotFound();
            }

            return View(Invoice);
        }
        [Authorize(Roles = "Manager")]
        [HttpGet]
        // GET: Admin/Invoice/Create
        public IActionResult Create()
        {
            ViewBag.Supplier = new SelectList(_context.Suppliers, "SupplierId", "SupplierName");
            ViewBag.Product = new SelectList(_context.Products, "ProductId", "ProductName");
            ViewBag.ListProduct = _context.ProductItems.Include(n => n.Product).ToList();
            ViewBag.Color = new SelectList(_context.Colors, "ColorId", "ColorName");
            ViewBag.ListSize = _context.ProductVariations.ToList();
            ViewBag.productItem = new SelectList(_context.ProductVariations, "ProductVarID", "ProductVarID");
            ViewBag.CreateDate = DateTime.Today;
            return View();
        }
        private Task<AppUserModel> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
        [Authorize(Roles = "Manager")]
        // POST: Admin/Invoice/Create
        // To protect from overposting attacks, enable the specific proper ties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> CreateAsync([Bind("InvoiceId,CreateDate,SupplierId")] Invoice model, IEnumerable<InvoiceDetail> lstModel)
        {

            var user = await GetCurrentUserAsync();
          
            var customerId = _context.Employees.Where(n => n.AccountId == user.Id).Select(n => n.EmployeeId).FirstOrDefault();
            model.EmployeeId = customerId;
          _context.Invoices.Add(model);

            _context.SaveChanges();
          
            ProductVariation product;
            foreach (var item in lstModel)
            {
                product = _context.ProductVariations.SingleOrDefault(n => n.ProductVarId == item.ProductVarId);
                if (product.QtyinStock == 0 || product.QtyinStock == null)
                {
                    product.QtyinStock = item.Quanity;
                }
                else
                {
                    product.QtyinStock += item.Quanity;
                }
                item.InvoiceId = model.InvoiceId;
         
            }
            _context.InvoiceDetails.AddRange(lstModel);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        // GET: Admin/Invoice/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierId", invoice.SupplierId);
            return View(invoice);
        }

        // POST: Admin/Invoice/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InvoiceId,CreateDate,SupplierId")] Invoice _Invoice)
        {
            if (id != _Invoice.InvoiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(_Invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(_Invoice.InvoiceId))
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
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierId", _Invoice.SupplierId);
            return View(_Invoice);
        }

        // GET: Admin/Invoice/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var Invoice = await _context.Invoices
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.InvoiceId == id);
            if (Invoice == null)
            {
                return NotFound();
            }

            return View(Invoice);
        }

        // POST: Admin/Invoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Invoices == null)
            {
                return Problem("Entity set 'LaptopContext.Invoices'  is null.");
            }
            var Invoice = await _context.Invoices.FindAsync(id);
            if (Invoice != null)
            {
                _context.Invoices.Remove(Invoice);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        public ActionResult DSSPHetHang()
        {
            var lstSP = _context.ProductVariations
                .Where(n => n.QtyinStock <= 5)
                .Include(n => n.ProductItems.Product)
                .Include(n => n.Ram)
                .Include(n => n.Ssd)
                .ToList();
            return View(lstSP);
        }

        [HttpGet]
        public ActionResult NhapHangDon(int? id)
        {
            ViewBag.Suppliers = new SelectList(_context.Suppliers.OrderBy(n => n.SupplierName), "SupplierId", "SupplierName");

            if (id == null)
            {
                return NotFound();
            }

            ViewBag.Product = _context.ProductVariations
                .Include(n => n.ProductItems.Product)
                .Include (n => n.ProductItems.Color)
                .Include(n => n.Ram)
                .Include(n => n.Ssd)
                ////.Include(n => n.Size)
                .SingleOrDefault(n => n.ProductVarId == id);

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> NhapHangDon(InvoiceViewModel model)
        {


            Invoice invoice = new Invoice()
            {
                CreateDate = DateTime.Now,
                SupplierId = model.SupplierId
            };

            _context.Invoices.Add(invoice);
            _context.SaveChanges();


            InvoiceDetail invoiceDetail = new InvoiceDetail()
            {
                ProductVarId = model.ProductVarId,
                InvoiceId = invoice.InvoiceId,
                Quanity = model.Quantity,
                Price = model.Price,
            };
            var product = _context.ProductVariations.Where(n => n.ProductVarId == model.ProductVarId).FirstOrDefault();
            await _context.InvoiceDetails.AddAsync(invoiceDetail);
            product.QtyinStock += invoiceDetail.Quanity;
            await _context.SaveChangesAsync();
          
         


            return RedirectToAction("Index","Invoice");
        }

     
        public async Task<JsonResult> GetProductByColorAsync(int id)
        {   
            var list = _context.ProductItems.Where(n => n.ProductId == id)
                          .Include(n => n.Color)
                .ToList();
            return Json(list);    
        }
        public async Task<JsonResult> GetRam(int id)
        {
            var productVariations = await _context.ProductVariations
                                .Where(pv => pv.ProductItems.ProductItemsId == id)
                                .Select(n => new {
                                    ProductVarId = n.ProductVarId,
                                    Ram = new { n.RamId, n.Ram.RamName }, // Chọn các thuộc tính cần thiết của Ram
                                    Ssd = new { n.Ssdid, n.Ssd.Ssdname }  // Chọn các thuộc tính cần thiết của Ssd
                                })
                                .ToListAsync();
            return Json(productVariations);
        }
        public async Task<JsonResult> GetSSD(int id,int ram)
        {
            var productVariations = await _context.ProductVariations
                                .Where(pv => pv.ProductItems.ProductItemsId == id && pv.RamId == ram)
                                .Select(n => new {
                                    ProductVarId = n.ProductVarId,           
                                    Ssd = new { n.Ssdid, n.Ssd.Ssdname }  
                                })
                                .ToListAsync();
            return Json(productVariations);
        }
        public async Task<JsonResult> GetProductByRamAsync(int id)
        {
            var list = await _context.ProductVariations
                .Where(n => n.ProductItemsId == id)
                .Select(n => new {
                    ProductVarId = n.ProductVarId,
                    Ram = new { n.RamId, n.Ram.RamName }, // Chọn các thuộc tính cần thiết của Ram
                    Ssd = new { n.Ssdid, n.Ssd.Ssdname }  // Chọn các thuộc tính cần thiết của Ssd
                })
                .ToListAsync();
            return Json(list);
        }
        public async Task<JsonResult> GetProductBySSDAsync(int id,int ram)
        {
            var list = await _context.ProductVariations
                                .Where(pv => pv.ProductItems.ProductItemsId == id && pv.RamId == ram)
                                .Select(n => new {
                                    ProductVarId = n.ProductVarId,
                                    Ssd = new { n.Ssdid, n.Ssd.Ssdname }
                                })
                .ToListAsync();
            return Json(list);
        }


        private bool InvoiceExists(int id)
        {
          return (_context.Invoices?.Any(e => e.InvoiceId == id)).GetValueOrDefault();
        }
    }
}
