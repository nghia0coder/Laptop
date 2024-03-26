using Laptop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Laptop.Components
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly LaptopContext _context;
        public MenuViewComponent(LaptopContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var lstSP = _context.Products
               .Include(n => n.BrandNavigation)
               .Include(n => n.Category);
            return View(lstSP);
        }
    }
}
