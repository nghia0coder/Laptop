﻿using Laptop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Laptop.Components
{
	public class NavViewComponent : ViewComponent
	{
		private LaptopContext _context;
		public NavViewComponent(LaptopContext context)
		{
			_context = context;
		}
		public IViewComponentResult Invoke()
		{
			var listCategory = _context.Brands
				.ToList();
			return View(listCategory);
		}
	}
}
