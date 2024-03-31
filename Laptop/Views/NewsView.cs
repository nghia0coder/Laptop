using Laptop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Laptop.Controllers
{
    public class Newsview : Controller
    {
        private readonly LaptopContext _context;
        public Newsview(LaptopContext context)
        {
            _context = context;
        }
        public ActionResult Show(int id)
        {
            // Lấy bài viết từ cơ sở dữ liệu dựa trên id
            var article = _context.News.FirstOrDefault(a => a.PostId == id);

            // Trả về view với bài viết đã lấy được
            return View(article);
        }



        public async Task<IActionResult> Index([Bind(Prefix = "page")] int pageNumber)
        {
            ViewBag.Brandname = _context.Brands.ToList();
            var posts = _context.News
            .Include(p => p.Author) // Load Author cho post  
            .Include(p => p.Brand) // Load các Category của Post
            .ThenInclude(c => c.BrandName)
            .AsQueryable();
            var Brandnames = GetBrandNames();
            var laptopContext = _context.News.Include(t => t.Brand);

            ViewData["Brandnames"] = Brandnames;
            return View(await laptopContext.ToListAsync());
        }
        public IActionResult GetBrandNames()
        {

            var brandNames = _context.Brands.Select(b => b.BrandName).ToList();
            return Json(brandNames);
        }
        public async Task<IActionResult> FilterByBrand(int brandid)
        {
            // Lọc các tin tức dựa trên brandName
            var filteredPosts = await _context.News
                .Include(p => p.Brand)
                .Where(p => p.Brand.BrandId == brandid)
                .ToListAsync();

            // Trả về view chứa các tin tức đã lọc
            return View("Index", filteredPosts);
        }
        
        
        public async Task<IActionResult> DisplayPost(int PostId)
        {

            ViewBag.Brandname = _context.Brands.ToList();
            // Truy vấn lấy bài viết theo ID
            var post = await _context.News
                .Where(p => p.PostId == PostId)
                .Include(p => p.Brand)
                .FirstOrDefaultAsync();
            var Brandnames = GetBrandNames();
                var laptopContext = _context.News.Include(t => t.Brand);
                return View(post);
        }
        public async Task<IActionResult> Displaybrandpost(int brandid)
        {
            if (brandid != null)
            {
                ViewBag.Brandname = _context.Brands.ToList();
                // Truy vấn lấy bài viết theo ID
                var post = await _context.News
                    .Where(p => p.BrandId == brandid)
                    .Include(p => p.Brand)
                    .ToListAsync();              
                var laptopContext = _context.News.Include(t => t.Brand);
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