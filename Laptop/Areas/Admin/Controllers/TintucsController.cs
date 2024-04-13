using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laptop.Models;
using Laptop.ViewModels;
using System.Security.Claims;

namespace Laptop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TintucsController : Controller
    {
        private readonly LaptopContext _context;
        private readonly IWebHostEnvironment _webHost;

        public TintucsController(LaptopContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }
        public ActionResult Show(int id)
        {
            // Lấy bài viết từ cơ sở dữ liệu dựa trên id
            var article = _context.Tintucs.FirstOrDefault(a => a.PostID == id);

            // Trả về view với bài viết đã lấy được
            return View(article);
        }

        // GET: Admin/Tintucs
        public async Task<IActionResult> Index()
        {
            
            var laptopContext =  _context.Tintucs.Include(t => t.Brand).ToList();
            return View(laptopContext);
        }
        public async Task<IActionResult> Displayindex()
        {
            var customerid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var laptopContext = _context.Tintucs
                .Where(t=> t.CustomerId != customerid)
                .Include(t => t.Brand)
                .ToList();
            return View(laptopContext);
        }
        // GET: Admin/Tintucs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tintucs == null)
            {
                return NotFound();
            }

            var tintuc = await _context.Tintucs
                .Include(t => t.Brand)
                .FirstOrDefaultAsync(m => m.PostID == id);
            if (tintuc == null)
            {
                return NotFound();
            }

            return View(tintuc);
        }

        // GET: Admin/Tintucs/Create
        public IActionResult Create()
        {
            ViewData["BrandID"] = new SelectList(_context.Brands, "BrandId", "BrandName");
            return View();
        }

        // POST: Admin/Tintucs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Tintuc tintuc)
        {
            string uniqueFileName1 = GetProfilePhotoFileName1(tintuc);
            tintuc.Thumburl = uniqueFileName1;
            var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            tintuc.CustomerId = userId;
            await _context.AddAsync(tintuc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
       
            return View(tintuc);
        }

        private string GetProfilePhotoFileName1(Tintuc Product)
        {
            string uniqueFileName = null;

            if (Product.Img1 != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "Contents/img/");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Product.Img1.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Product.Img1.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        // GET: Admin/Tintucs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null || _context.Tintucs == null)
            {
                return NotFound();
            }

            var tintuc = await _context.Tintucs.FindAsync(id);
            if (tintuc == null)
            {
                return NotFound();
            }
            ViewData["BrandID"] = new SelectList(_context.Brands, "BrandId", "BrandName", tintuc.BrandID);
            return View(tintuc);
        }
        public async Task<IActionResult> Userpost(int? id)
        {

            if (id == null || _context.Tintucs == null)
            {
                return NotFound();
            }

            var tintuc = await _context.Tintucs.FindAsync(id);
            if (tintuc == null)
            {
                return NotFound();
            }
            ViewData["BrandID"] = new SelectList(_context.Brands, "BrandId", "BrandName", tintuc.BrandID);
            return View(tintuc);
        }

        // POST: Admin/Tintucs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostID,Title,Contentspreview,Contents,Thumb,Thumburl,Status,Author,CreatedDate,Hot,New,BrandID,CustomerId")] Tintuc tintuc)
        {
            if (id != tintuc.PostID)
            {
                return NotFound();
            }

            string image1 = GetProfilePhotoFileName1(tintuc);
            if (!String.IsNullOrEmpty(image1))
            {
                string uniqueFileName1 = image1;
                tintuc.Thumburl = uniqueFileName1;


            }
            _context.Update(tintuc);
            _context.SaveChanges();

            ViewData["BrandID"] = new SelectList(_context.Brands, "BrandId", "BrandName", tintuc.BrandID);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Userpost(int id, [Bind("PostID,Title,Contentspreview,Contents,Thumb,Thumburl,Status,Author,CreatedDate,Hot,New,BrandID,CustomerID")] Tintuc tintuc)
        {
            if (id != tintuc.PostID)
            {
                return NotFound();
            }


            try
            {
                string uniqueFileName1 = GetProfilePhotoFileName1(tintuc);
                tintuc.Thumburl = uniqueFileName1;
                _context.Update(tintuc);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TintucExists(tintuc.PostID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }
            ViewData["BrandID"] = new SelectList(_context.Brands, "BrandId", "BrandId", tintuc.BrandID);
            return View(tintuc);
        }
        // GET: Admin/Tintucs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tintucs == null)
            {
                return NotFound();
            }

            var tintuc = _context.Tintucs
                .Include(t => t.Brand)
                .FirstOrDefault(m => m.PostID == id);
            if (tintuc == null)
            {
                return NotFound();
            }

            return View(tintuc);
        }


        // POST: Admin/Tintucs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tintucs == null)
            {
                return Problem("Entity set 'LaptopContext.Tintucs'  is null.");
            }
            var tintuc = await _context.Tintucs.FindAsync(id);
            if (tintuc != null)
            {
                _context.Tintucs.Remove(tintuc);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TintucExists(int id)
        {
          return (_context.Tintucs?.Any(e => e.PostID == id)).GetValueOrDefault();
        }
    }
}
