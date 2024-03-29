using Laptop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;


namespace Laptop.Controllers
{	
	
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LaptopContext _context;
       

        public HomeController(ILogger<HomeController> logger, LaptopContext context)
        {
            _logger = logger;
            _context = context;
        }

        public ActionResult Index()
        {
	
			var lstLTM = _context.ProductVariations
				 .Include(n => n.ProductItems.Product.Category)
                 .Include(n => n.Ram)
			     .Include(n => n.Ssd)
				 .OrderBy(n => Guid.NewGuid())
				 .ToList()
                 .GroupBy(n => n.ProductItems.Product.ProductName) 
	             .Select(group => group.First()) 
	              .ToList();

			//Gán vào viewbag
			ViewBag.ListLTM = lstLTM;

            var lstSelling = _context.ProductItems
                .Where(n => n.ProductId == 2)
                .Include(n => n.Product.Category)
                .Include(n => n.ProductVariations)
                .ToList();
            //Gán vào viewbag
            ViewBag.ListSelling = lstSelling;

            //List CategoryId bằng 3
            var lstDTM = _context.ProductVariations
                .Include(n => n.ProductItems.Product.Category)
				 .Include(n => n.Ram)
				 .Include(n => n.Ssd)
				.Include(n => n.ProductItems.Product.BrandNavigation)
                .OrderBy(n => Guid.NewGuid())
                .ToList()
                .GroupBy(n => n.ProductItems.Product.ProductName)
				 .Select(group => group.First())
				  .ToList();
			//Gán vào viewbag
			ViewBag.ListDTM = lstDTM;

            


            return View();
        }
        public IActionResult Footer ()
        {
			var listCate = _context.Categories.ToList();

			return View("~/Shared/Partial/_FooterPartial.cshtml", listCate);
		}
        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
