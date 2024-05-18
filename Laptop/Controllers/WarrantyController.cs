using Microsoft.AspNetCore.Mvc;

namespace Laptop.Controllers
{
    public class WarrantyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult WarrantyInfo()
        {
            return View();
        }
    }
}
