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
        [HttpGet]
        public IActionResult OrderConfirm(string? orderid, int pg = 1)
        {

            if (orderid != null)
            {
                var lst = _context.Orders
              .Where(n => n.OrderStatus == 1 && n.OrderId.ToString().Contains(orderid))
              .Include(n => n.StatusNaviagtion)
              .Include(n => n.Customer)
              .OrderBy(n => n.OrderDate);



                const int pageSize = 5;
                if (pg < 1)
                    pg = 1;

                var totalCate = _context.Orders.Where(n => n.OrderStatus == 1 && n.OrderId.ToString().Contains(orderid)).Include(n => n.Customer).Count();

                var pager = new Page(totalCate, pg, pageSize);

                int recSkip = (pg - 1) * pageSize;

                var data = lst.Skip(recSkip).Take(pageSize).ToList();


                ViewBag.Search = orderid;
                ViewBag.Page = pager;
                return View(data);

            }
            else
            {
                var lst = _context.Orders
                      .Where(n => n.OrderStatus == 1)
                      .Include(n => n.StatusNaviagtion)
                      .Include(n => n.Customer)
                      .OrderBy(n => n.OrderDate);

                const int pageSize = 5;
                if (pg < 1)
                    pg = 1;

                var totalCate = _context.Orders.Where(n => n.OrderStatus == 1).Include(n => n.Customer).Count();

                var pager = new Page(totalCate, pg, pageSize);

                int recSkip = (pg - 1) * pageSize;

                var data = lst.Skip(recSkip).Take(pageSize).ToList();

                ViewBag.Page = pager;
                return View(data);

            }
        }
        [HttpGet]
        public IActionResult Delivering(string orderid, int pg = 1)
        {

            if (orderid != null)
            {
                var lstDSDHCG = _context.Orders
                .Include(n => n.Customer)
                .Include(n => n.Employee)
                .Where(n => n.OrderStatus == 2 && n.OrderId.ToString().Contains(orderid))
                .Include(n => n.StatusNaviagtion)
                .OrderBy(n => n.DeliveryDate)
                .ToList();

                const int pageSize = 5;
                if (pg < 1)
                    pg = 1;

                var totalCate = _context.Orders.Where(n => n.OrderStatus == 2 && n.OrderId.ToString().Contains(orderid)).Include(n => n.Customer).Count();

                var pager = new Page(totalCate, pg, pageSize);

                int recSkip = (pg - 1) * pageSize;

                var data = lstDSDHCG.Skip(recSkip).Take(pageSize).ToList();

                ViewBag.Page = pager;

                return View(data);
            }
            else
            {
                var lstDSDHCG = _context.Orders
                .Include(n => n.Customer)
                .Include(n => n.Employee)
                .Where(n => n.OrderStatus == 2)
                .Include(n => n.StatusNaviagtion)
                .OrderBy(n => n.DeliveryDate)
                .ToList();

                const int pageSize = 5;
                if (pg < 1)
                    pg = 1;

                var totalCate = _context.Orders.Where(n => n.OrderStatus == 2).Include(n => n.Customer).Count();

                var pager = new Page(totalCate, pg, pageSize);

                int recSkip = (pg - 1) * pageSize;

                var data = lstDSDHCG.Skip(recSkip).Take(pageSize).ToList();

                ViewBag.Page = pager;

                return View(data);
            }

        }
        [HttpGet]
        public IActionResult Cancel(string orderid, int pg = 1)
        {
            if (orderid == null)
            {
                var lstDSDHCG = _context.Orders
                    .Include(n => n.Customer)
                    .Where(n => n.OrderStatus == 4)
                    .Include(n => n.StatusNaviagtion)
                    .OrderBy(n => n.DeliveryDate)
                    .ToList();

                const int pageSize = 5;
                if (pg < 1)
                    pg = 1;

                var totalCate = _context.Orders.Where(n => n.OrderStatus == 4).Include(n => n.Customer).Count();

                var pager = new Page(totalCate, pg, pageSize);

                int recSkip = (pg - 1) * pageSize;

                var data = lstDSDHCG.Skip(recSkip).Take(pageSize).ToList();

                ViewBag.Page = pager;
                return View(data);
            }
            else
            {
                var lstDSDHCG = _context.Orders
               .Include(n => n.Customer)
               .Where(n => n.OrderStatus == 4 && n.OrderId.ToString().Contains(orderid))
               .Include(n => n.StatusNaviagtion)
               .OrderBy(n => n.DeliveryDate)
               .ToList();

                const int pageSize = 5;
                if (pg < 1)
                    pg = 1;

                var totalCate = _context.Orders.Where(n => n.OrderStatus == 4 && n.OrderId.ToString().Contains(orderid)).Include(n => n.Customer).Count();

                var pager = new Page(totalCate, pg, pageSize);

                int recSkip = (pg - 1) * pageSize;

                var data = lstDSDHCG.Skip(recSkip).Take(pageSize).ToList();

                ViewBag.Page = pager;
                return View(data);
            }

        }
        public IActionResult Delivered(string orderid, int pg = 1)
        {
            if (orderid == null)
            {
                var lstDSDHCG = _context.Orders
                             .Include(n => n.Customer)
                             .Include(n => n.Employee)
                             .Include(n => n.StatusNaviagtion)
                             .Where(n => n.OrderStatus == 3)
                             .OrderBy(n => n.DeliveryDate)
                             .ToList();

                const int pageSize = 5;
                if (pg < 1)
                    pg = 1;

                var totalCate = _context.Orders.Where(n => n.OrderStatus == 3).Include(n => n.Customer).Count();

                var pager = new Page(totalCate, pg, pageSize);

                int recSkip = (pg - 1) * pageSize;

                var data = lstDSDHCG.Skip(recSkip).Take(pageSize).ToList();

                ViewBag.Page = pager;
                return View(data);
            }
            else
            {
                var lstDSDHCG = _context.Orders
                             .Include(n => n.Customer)
                             .Include(n => n.Employee)
                             .Include(n => n.StatusNaviagtion)
                             .Where(n => n.OrderStatus == 3 && n.OrderId.ToString().Contains(orderid))
                             .OrderBy(n => n.DeliveryDate)
                             .ToList();

                const int pageSize = 5;
                if (pg < 1)
                    pg = 1;

                var totalCate = _context.Orders.Where(n => n.OrderStatus == 3 && n.OrderId.ToString().Contains(orderid)).Include(n => n.Customer).Count();

                var pager = new Page(totalCate, pg, pageSize);

                int recSkip = (pg - 1) * pageSize;

                var data = lstDSDHCG.Skip(recSkip).Take(pageSize).ToList();

                ViewBag.Page = pager;
                return View(data);
            }

        }
        public async Task<IActionResult> DeliveredConfirm(string id)
        {
            var order = await _context.Orders.FindAsync(id);
            order.OrderStatus = 3;
            if (!order.StatusPayment)
            {
                order.StatusPayment = true;
            }
            order.DeliveryDate = DateTime.Now;
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
        [Route("/orders/{id}/{detail:int?}")]
        public IActionResult ApproveOrders(string? id, int? detail)
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

            ViewBag.TenKH = model.Customer?.Name;

            ViewBag.Detail = detail;

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

            ViewBag.TenKH = model.Customer?.Name;

            return View(model);
        }

        [HttpPost]
        [Route("/orders/{id}/{detail:int?}")]

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

            ViewBag.TenKH = model.Customer?.Name;

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
