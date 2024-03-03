using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GiayDep.Models;

namespace GiayDep.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HoaDonController : Controller
    {
        private readonly LaptopContext _context;

        public HoaDonController(LaptopContext context)
        {
            _context = context;
        }

        public IActionResult ChuaThanhToan()
        {
            var lst = _context.HoaDons.Include(n => n.MakhNavigation).Where(n => !n.Dathanhtoan).OrderBy(n => n.Ngaythanhtoan).ToList();
            return View(lst);
        }
         
        public IActionResult ChuaGiao()
        {
            var lstDSDHCG = _context.HoaDons
                .Include(n => n.MakhNavigation)
                .Where(n => !n.Tinhtranggiaohang && n.Dathanhtoan)
                .OrderBy(n => n.Ngaygiao)
                .ToList();
            return View(lstDSDHCG);
        }

        public IActionResult DaGiaoDaThanhToan()
        {
            var lstDSDHCG = _context.HoaDons
                .Include(n => n.MakhNavigation)
                .Where(n => n.Tinhtranggiaohang && n.Dathanhtoan)
                .OrderBy(n => n.Ngaygiao)
                .ToList();
            return View(lstDSDHCG);
        }

        [HttpGet]
        public IActionResult DuyetDonHang(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            HoaDon model = _context.HoaDons
                .Include(n => n.MakhNavigation)
                .SingleOrDefault(n => n.Idhoadon == id);

            if (model == null)
            {
                return NotFound();
            }

            var lstChiTietDH = _context.CtHoaDons
                .Where(n => n.Idhoadon == id)
                .Include(ct => ct.IdspNavigation)
                .ToList();

            ViewBag.ListChiTietDH = lstChiTietDH;
            ViewBag.TenKH = model.MakhNavigation.UserName;

            return View(model);
        }

        [HttpPost]
        public IActionResult DuyetDonHang(HoaDon ddh)
        {
            HoaDon ddhUpdate = _context.HoaDons.Single(n => n.Idhoadon == ddh.Idhoadon);
            ddhUpdate.Dathanhtoan = ddh.Dathanhtoan;
            ddhUpdate.Tinhtranggiaohang = ddh.Tinhtranggiaohang;
            _context.SaveChanges();

            var lstChiTietDH = _context.CtHoaDons
                .Where(n => n.Idhoadon == ddh.Idhoadon)
                .Include(n => n.IdspNavigation)
                .ToList();

            ViewBag.ListChiTietDH = lstChiTietDH;

            return View(ddhUpdate);
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
