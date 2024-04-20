using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laptop.Models;
using System.Security.Claims;

namespace Laptop.Areas.Admin.Controllers
{
    [Area("Admin")]
	public class OrderController : Controller
	{
		private readonly LaptopContext _context;

		public OrderController(LaptopContext context)
		{
			_context = context;
		}

		public IActionResult OrderConfirm()
		{
			var lst = _context.Orders
				.Include(n => n.StatusNaviagtion)
				.Include(n => n.Customer)
				.Where(n => n.OrderStatus == 1)
				.OrderBy(n => n.OrderDate)
				.ToList();
			return View(lst);
		}
		[HttpGet]
		public IActionResult Delivering()
		{
			var lstDSDHCG = _context.Orders
				.Include(n => n.Customer)
				.Where(n => n.OrderStatus == 2)
				.Include(n => n.StatusNaviagtion)
				.OrderBy(n => n.DeliveryDate)
				.ToList();
			return View(lstDSDHCG);
		}
		[HttpGet]
		public IActionResult Cancel()
		{
			var lstDSDHCG = _context.Orders
				.Include(n => n.Customer)
				.Where(n => n.OrderStatus == 4)
				.Include(n => n.StatusNaviagtion)
				.OrderBy(n => n.DeliveryDate)
				.ToList();
			return View(lstDSDHCG);
		}
		public IActionResult Delivered(int id)
		{
			var lstDSDHCG = _context.Orders
				.Include(n => n.Customer)
				.Include(n => n.StatusNaviagtion)
				.Where(n => n.OrderStatus == 3)
				.OrderBy(n => n.DeliveryDate)
				.ToList();
			return View(lstDSDHCG);
		}
		public async Task<IActionResult> DeliveredConfirm(int id)
		{
			var order = await _context.Orders.FindAsync(id);
			order.OrderStatus = 3;

			_context.Orders.Update(order);
			await _context.SaveChangesAsync();
			return RedirectToAction("Delivered");
		}
		[HttpGet]
		public IActionResult PaidDelivered()
		{
			var lstDSDHCG = _context.Orders
				.Include(n => n.Customer)
				.Where(n => n.StatusPayment)
				.OrderBy(n => n.DeliveryDate)
				.ToList();
			return View(lstDSDHCG);
		}

		[HttpGet]
		public IActionResult ApproveOrders(string? id)
		{
			if (id == null)
			{
				return new BadRequestResult();
			}

			Order model = _context.Orders
				.Include(n => n.Customer.Account)
				.SingleOrDefault(n => n.OrderId == id);

			if (model == null)
			{
				return NotFound();
			}


			ViewBag.ListChiTietDH = _context.OrdersDetails
				.Where(n => n.OrderId == id)
				.Include(n => n.ProductVar.ProductItems.Product)
				.ToList();

			ViewBag.TenKH = model.Customer.Name;

			return View(model);
		}
		[HttpGet]
		public IActionResult DeliveryConfirm(string? id)
		{
			if (id == null)
			{
				return new BadRequestResult();
			}

			Order model = _context.Orders
				.Include(n => n.Customer.Account)
				.SingleOrDefault(n => n.OrderId == id);

			if (model == null)
			{
				return NotFound();
			}


			ViewBag.ListChiTietDH = _context.OrdersDetails
				.Where(n => n.OrderId == id)
				.Include(n => n.ProductVar.ProductItems.Product)
				.ToList();

			ViewBag.TenKH = model.Customer.Name;

			return View(model);
		}

		[HttpPost]
		public IActionResult ApproveOrders(Order ddh)
		{

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var employeeeID = _context.Employees.Where(n => n.AccountId == userId).Select(n => n.EmployeeId).FirstOrDefault();


			Order ddhUpdate = _context.Orders.FirstOrDefault(n => n.OrderId == ddh.OrderId);

			ddhUpdate.StatusPayment = ddh.StatusPayment;
			ddhUpdate.OrderStatus = 2;
			ddhUpdate.EmployeeId = employeeeID;
			_context.Orders.Update(ddhUpdate);
			_context.SaveChanges();

			//var lstChiTietDH = _context.OrdersDetails
			//    .Where(n => n.OrderId == ddh.OrderId)
			//    .Include(n => n.ProductVar)
			//    .ToList();

			//ViewBag.ListChiTietDH = lstChiTietDH;

			return RedirectToAction("Delivering", "Order");
		}
		[HttpGet]
		public IActionResult OrderCancel(string? id)
		{
			if (id == null)
			{
				return new BadRequestResult();
			}

			Order model = _context.Orders
				.Include(n => n.Customer.Account)
				.SingleOrDefault(n => n.OrderId == id);

			if (model == null)
			{
				return NotFound();
			}


			ViewBag.ListChiTietDH = _context.OrdersDetails
				.Where(n => n.OrderId == id)
				.Include(n => n.ProductVar.ProductItems.Product)
				.ToList();

			ViewBag.TenKH = model.Customer.Name;

			return View(model);
		}

		[HttpPost]
		public IActionResult OrderCancel(Order ddh)
		{
			Order ddhUpdate = _context.Orders.FirstOrDefault(n => n.OrderId == ddh.OrderId);

			ddhUpdate.StatusPayment = ddh.StatusPayment;
			ddhUpdate.OrderStatus = 4;
			_context.Orders.Update(ddhUpdate);
			_context.SaveChanges();

			//var lstChiTietDH = _context.OrdersDetails
			//    .Where(n => n.OrderId == ddh.OrderId)
			//    .Include(n => n.ProductVar)
			//    .ToList();

			//ViewBag.ListChiTietDH = lstChiTietDH;

			return RedirectToAction("Cancel", "Order");
		}

		// Dispose method
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_context != null)
				{
					_context.Dispose();
				}
			}
			base.Dispose(disposing);
		}
	}
}
