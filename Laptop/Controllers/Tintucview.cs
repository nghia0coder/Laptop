using Laptop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;

namespace Laptop.Controllers
{
    public class Tintucview : Controller
    {
        private readonly LaptopContext _context;
        private readonly IWebHostEnvironment _webHost;
        public Tintucview(LaptopContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }
        



        public async Task<IActionResult> Index(int pg=1)
        {
            ViewBag.Brandname = _context.Brands.ToList();
      
       
            var lst = _context.Tintucs
                .Include(t => t.Brand)
                .OrderByDescending(p => p.CreatedDate)
                .ToList();

            const int pageSize = 7;
            if (pg < 1)
                pg = 1;
            int recsCount = lst.Count();

            var pager = new Pager(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            var data = lst.Skip(recSkip).Take(pageSize).ToList();

            this.ViewBag.Pager = pager;
            return View(data);
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
            var post = _context.Tintucs
                .Where(p => p.PostID == postid)
                .Include(p => p.Brand)
                .Include(p=> p.PostComments).ThenInclude(p=>p.Customer)
                .Include(p=> p.Customer)
                .OrderByDescending(p => p.CreatedDate)
                .FirstOrDefault();
            
            
            //var laptopContext = _context.Tintucs.Include(t => t.Brand);
            return View(post);
        }
        public async Task<IActionResult> Displaybrandpost(int brandid)
        {
            if (brandid != null)
            {
                ViewBag.Brandname = _context.Brands.ToList();
                // Truy vấn lấy bài viết theo ID
                var post = _context.Tintucs
                    .Where(p => p.BrandId == brandid)
                    .Include(p => p.Brand)
                    .OrderByDescending(p => p.CreatedDate)
                    .ToList();              
                var laptopContext = _context.Tintucs.Include(t => t.Brand);
                return View(post);
            }
            if (brandid == null)
            {
                return Content("Error");
            }
            return View();

        }

        [HttpGet]
        public IActionResult Createpost()
        {
            ViewData["BrandID"] = new SelectList(_context.Brands, "BrandId", "BrandName");
            ViewBag.Brandname = _context.Brands.ToList();
            var laptopContext = _context.Tintucs.Include(t => t.Brand);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Createpost(Tintuc tintuc)
        {
            string uniqueFileName1 = GetProfilePhotoFileName1(tintuc);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customerId = _context.Customers.Where(n => n.AccountId ==  userId).Select(n => n.CustomerId).FirstOrDefault();
            tintuc.Customer.CustomerId = customerId;

            tintuc.Thumburl = uniqueFileName1;
            tintuc.Status = false;
            tintuc.Hot = false;
            tintuc.New = false;
            tintuc.CreatedDate = DateTime.Now;
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
        public async Task<IActionResult> Displayuserpost()
        {
            ViewBag.Brandname = _context.Brands.ToList();
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customerId = _context.Customers.Where(n => n.AccountId == userID).Select(n => n.CustomerId).FirstOrDefault();


            // Truy vấn lấy bài viết theo ID
            var post = _context.Tintucs
                .Where(p => p.Author == customerId && p.Status)
                .Include(p => p.Brand)
                .OrderByDescending(p => p.CreatedDate)
                .ToList();

            return View(post);
        }
        public async Task<IActionResult> Displayuserunpost()
        {
            ViewBag.Brandname = _context.Brands.ToList();
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customerId = _context.Customers.Where(n => n.AccountId == userID).Select(n => n.CustomerId).FirstOrDefault();
            // Truy vấn lấy bài viết theo ID
            var post = _context.Tintucs
                .Where(p => p.Author == customerId && !p.Status)
                .Include(p => p.Brand)
                .OrderByDescending(p => p.CreatedDate)
                .ToList();

            return View(post);
        }
    }
}