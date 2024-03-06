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
            var lstSP = _context.SanPhams
               .Include(n => n.ManhaccNavigation)
               .Include(n => n.MaloaispNavigation);
            return View(lstSP);
        }
    }
}
