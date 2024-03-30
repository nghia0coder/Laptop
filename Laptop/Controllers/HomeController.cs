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

            var lstLTM = _context.Products
                 .Include(p => p.ProductItems)
                    .ThenInclude(pi => pi.ProductVariations)
                        .ThenInclude(pv => pv.Ram)
                 .Include(p => p.ProductItems)
                    .ThenInclude(pi => pi.ProductVariations)
                        .ThenInclude(pv => pv.Ssd)
                 .Include(n => n.Category)
                 .ToList()
                 .GroupBy(p => p.ProductName)
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
            var lstDTM = _context.Products
				 .Include(p => p.ProductItems)
					.ThenInclude(pi => pi.ProductVariations)
						.ThenInclude(pv => pv.Ram)
				 .Include(p => p.ProductItems)
					.ThenInclude(pi => pi.ProductVariations)
						.ThenInclude(pv => pv.Ssd)
				.Include(n => n.Category)
                .ToList()
				  .GroupBy(p => p.ProductName)
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
