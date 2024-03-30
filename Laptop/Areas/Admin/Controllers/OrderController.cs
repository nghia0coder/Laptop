using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laptop.Models;

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

        public IActionResult Unpaid(int pg = 1)
        {
            var lst = _context.Orders.Include(n => n.Customer).Where(n => !n.Status).OrderBy(n => n.OrderDate).ToList();

            const int pageSize = 5;
            if (pg < 1)
                pg = 1;
            int recsCount = lst.Count();

            var pager = new Pager(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            var data = lst.Skip(recSkip).Take(pageSize).ToList();

            this.ViewBag.Pager = pager;

            //return View(lst);

            return View(data);
        }
        [HttpGet]
        public IActionResult PaidUndelivered(int pg = 1)
        {
            var lstDSDHCG = _context.Orders.Include(n => n.Customer).Where(n => !n.Status).OrderBy(n => n.OrderDate).ToList();

            const int pageSize = 5;
            if (pg < 1)
                pg = 1;
            int recsCount = lstDSDHCG.Count();

            var pager = new Pager(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            var data = lstDSDHCG.Skip(recSkip).Take(pageSize).ToList();

            this.ViewBag.Pager = pager;

            //return View(lst);

            return View(data);
        }
        [HttpGet]
        public IActionResult PaidDelivered(int pg=1)
        {
            var lstDSDHCG = _context.Orders.Include(n => n.Customer).Where(n => !n.Status).OrderBy(n => n.OrderDate).ToList();

            const int pageSize = 5;
            if (pg < 1)
                pg = 1;
            int recsCount = lstDSDHCG.Count();

            var pager = new Pager(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            var data = lstDSDHCG.Skip(recSkip).Take(pageSize).ToList();

            this.ViewBag.Pager = pager;

            //return View(lst);

            return View(data);
        }

        [HttpGet]
        public IActionResult ApproveOrders(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            Order model = _context.Orders
                .Include(n => n.Customer)
                .SingleOrDefault(n => n.OrderId == id);

            if (model == null)
            {
                return NotFound();
            }


            ViewBag.ListChiTietDH = _context.OrdersDetails
                .Where(n => n.OrderId == id)
                .Include(n => n.ProductVar.ProductItems.Product)
                .ToList(); 

            ViewBag.TenKH = model.Customer.UserName;

            return View(model);
        }

        [HttpPost]
        public IActionResult ApproveOrders(Order ddh)
        {
            Order ddhUpdate = _context.Orders.FirstOrDefault(n => n.OrderId == ddh.OrderId);
            if (ddhUpdate == null)
            {
                return Content("Contet");
            }
            ddhUpdate.Status = ddh.Status;
            ddhUpdate.Delivered = ddh.Delivered;
            _context.Orders.Update(ddhUpdate);
            _context.SaveChanges();

            //var lstChiTietDH = _context.OrdersDetails
            //    .Where(n => n.OrderId == ddh.OrderId)
            //    .Include(n => n.ProductVar)
            //    .ToList();

            //ViewBag.ListChiTietDH = lstChiTietDH;

            return RedirectToAction("PaidUndelivered","Order");
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
