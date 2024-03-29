using Laptop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Laptop.Components
{
	public class FooterViewComponent : ViewComponent
	{
		private readonly LaptopContext _context;
		public FooterViewComponent(LaptopContext context)
		{
			_context = context;
		}
		public IViewComponentResult Invoke()
		{
			var lstSP = _context.Categories
			   .ToList();
			return View(lstSP);
		}
	}
}
