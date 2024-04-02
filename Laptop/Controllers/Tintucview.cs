using Laptop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Laptop.Controllers
{
    public class Tintucview : Controller
    {
        private readonly LaptopContext _context;
        public Tintucview(LaptopContext context)
        {
            _context = context;
        }
        



        public async Task<IActionResult> Index()
        {
            ViewBag.Brandname = _context.Brands.ToList();
      
       
            var laptopContext = _context.Tintucs
                .Include(t => t.Brand)
                .OrderByDescending(p => p.CreatedDate)
                .ToList();

            return View(laptopContext);
        }
        public IActionResult GetBrandNames()
        {

            var brandNames = _context.Brands.Select(b => b.BrandName).ToList();
            return Json(brandNames);
        }
        
        
        
        public async Task<IActionResult> DisplayPost(int postid)
        {

            ViewBag.Brandname = _context.Brands.ToList();
            // Truy vấn lấy bài viết theo ID
            var post = await _context.Tintucs
                .Where(p => p.PostID == postid)
                .Include(p => p.Brand)
                .OrderByDescending(p => p.CreatedDate)
                .FirstOrDefaultAsync();
   
            var laptopContext = _context.Tintucs.Include(t => t.Brand);
            return View(post);
        }
        public async Task<IActionResult> Displaybrandpost(int brandid)
        {
            if (brandid != null)
            {
                ViewBag.Brandname = _context.Brands.ToList();
                // Truy vấn lấy bài viết theo ID
                var post = await _context.Tintucs
                    .Where(p => p.BrandID == brandid)
                    .Include(p => p.Brand)
                    .OrderByDescending(p => p.CreatedDate)
                    .ToListAsync();              
                var laptopContext = _context.Tintucs.Include(t => t.Brand);
                return View(post);
            }
            if (brandid == null)
            {
                return Content("Error");
            }
            return View();

        }

    }
}