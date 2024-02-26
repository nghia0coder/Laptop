using GiayDep.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;

namespace GiayDep.Controllers
{	
	//change
    //nghia
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
            //Lần lượt tạo các viewbag để lấy list sp từ csdl
            //List maloaisp bằng 1
            var lstLTM = _context.SanPhams
                .Where(n => n.Maloaisp == 1 )
                .Include(n => n.MaloaispNavigation)
                .ToList();
            //Gán vào viewbag
            ViewBag.ListLTM = lstLTM;

			var lstSelling = _context.SanPhams
				.Where(n => n.Maloaisp == 3)
				.Include(n => n.MaloaispNavigation)
				.ToList();
			//Gán vào viewbag
			ViewBag.ListSelling = lstSelling;

			//List maloaisp bằng 3
			var lstDTM = _context.SanPhams
                .Where(n => n.Maloaisp == 3)
                .Include(n => n.MaloaispNavigation)
                .ToList();
            //Gán vào viewbag
            ViewBag.ListDTM = lstDTM;


            return View();
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
