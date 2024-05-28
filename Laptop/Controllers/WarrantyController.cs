using Laptop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Laptop.Controllers
{
    public class WarrantyController : Controller
    {
        private readonly LaptopContext _context;

        public WarrantyController(LaptopContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult WarrantyInfo(int warrantyId)
        {
            if (warrantyId <= 0)
            {
                ViewBag.Error = "Vui lòng nhập Warranty ID.";
                return View(new Warranty());
            }

            var warranty = _context.Warranties
                .Include(p => p.Order)
                .ThenInclude(o => o.OrdersDetails)
                .ThenInclude(od => od.ProductVar)
                .FirstOrDefault(w => w.WarrantyId == warrantyId);

            if (warranty == null)
            {
                ViewBag.Error = "Không tìm thấy thông tin bảo hành cho Warranty ID này.";
                return View(new Warranty());
            }

            return View(warranty);
        }
    }
}