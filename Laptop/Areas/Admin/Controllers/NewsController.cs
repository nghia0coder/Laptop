using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laptop.Models;
using Laptop.ViewModels;

namespace Laptop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewsController : Controller
    {
        private readonly LaptopContext _context;
        private readonly IWebHostEnvironment _webHost;

        public NewsController(LaptopContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }
        public ActionResult Show(int id)
        {
            // Lấy bài viết từ cơ sở dữ liệu dựa trên id
            var article = _context.News.FirstOrDefault(a => a.PostId == id);

            // Trả về view với bài viết đã lấy được
            return View(article);
        }

        // GET: Admin/News
        public async Task<IActionResult> Index()
        {
            
            var laptopContext = _context.News.Include(t => t.Brand);
            return View(await laptopContext.ToListAsync());
        }

        // GET: Admin/News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var New = await _context.News
                .Include(t => t.Brand)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (New == null)
            {
                return NotFound();
            }

            return View(New);
        }

        // GET: Admin/News/Create
        public IActionResult Create()
        {
            ViewData["BrandID"] = new SelectList(_context.Brands, "BrandId", "BrandName");
            return View();
        }

        // POST: Admin/News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(News New)
        {
            string uniqueFileName1 = GetProfilePhotoFileName1(New);
            New.ThumbUrl = uniqueFileName1;
            await _context.AddAsync(New);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
       
            return View(New);
        }

        private string GetProfilePhotoFileName1(News Product)
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

        // GET: Admin/News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var New = await _context.News.FindAsync(id);
            if (New == null)
            {
                return NotFound();
            }
            ViewData["BrandID"] = new SelectList(_context.Brands, "BrandId", "BrandName", New.BrandId);
            return View(New);
        }

        // POST: Admin/News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,ContentPreview,Contents,Thumb,ThumbUrl,Status,Author,CreatedDate,Hot,New,BrandID")] News New)
        {
            if (id != New.PostId)
            {
                return NotFound();
            }

          

            try
                {
                string uniqueFileName1 = GetProfilePhotoFileName1(New);
                New.ThumbUrl = uniqueFileName1;
                _context.Update(New);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewExists(New.PostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandID"] = new SelectList(_context.Brands, "BrandId", "BrandId", New.BrandId);
            return View(New);
        }

        // GET: Admin/News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var New = await _context.News
                .Include(t => t.Brand)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (New == null)
            {
                return NotFound();
            }

            return View(New);
        }

        // POST: Admin/News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.News == null)
            {
                return Problem("Entity set 'LaptopContext.News'  is null.");
            }
            var New = await _context.News.FindAsync(id);
            if (New != null)
            {
                _context.News.Remove(New);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewExists(int id)
        {
          return (_context.News?.Any(e => e.PostId == id)).GetValueOrDefault();
        }
    }
}
