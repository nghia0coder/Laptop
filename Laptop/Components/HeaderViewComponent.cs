using Laptop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Laptop.Components
{
	public class HeaderViewComponent : ViewComponent
	{
		private readonly LaptopContext _context;

		public HeaderViewComponent(LaptopContext context)
		{
			_context = context;
		}
		public IViewComponentResult Invoke()
		{
		
			ViewData["Category"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
			return View();
		}

	}
}
