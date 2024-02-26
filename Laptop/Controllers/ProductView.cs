using GiayDep.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GiayDep.Controllers
{
    public class ProductView : Controller
    {
        private readonly LaptopContext _context;

        public ProductView (LaptopContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
   
        public ActionResult SanPhamStyle1Partial()
        {
            return PartialView();
        }
        public ActionResult SanPhamStyle2Partial()
        {
            return PartialView();
        }
        public async Task<IActionResult> XemChiTiet(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var sp = await _context.SanPhams
                .Include(n => n.MaloaispNavigation)
                .SingleOrDefaultAsync(n => n.Idsp == id);
            ViewBag.ListSP = _context.SanPhams
                .Where(n => n.Maloaisp == sp.Maloaisp);
            if (sp == null)
            {
                return NotFound();
            }

            return View(sp);
        }
        public IActionResult ThuongHieu(int? MaLoaiSP, int? MaNSX)
        {
            // Check if the parameters are null
            if (MaLoaiSP == null || MaNSX == null)
            {
                return BadRequest();
            }

            // Load products based on the specified criteria
            var lstSP = _context.SanPhams.Where(n => n.Maloaisp == MaLoaiSP && n.Manhacc == MaNSX)
                .Include(n => n.MaloaispNavigation)
                .Include(n => n.ManhaccNavigation);

            // Check if there are any products
            if (lstSP.Count() == 0)
            {
                return NotFound();
            }

            ViewBag.MaLoaiSP = MaLoaiSP;
            ViewBag.MaNSX = MaNSX;

            // Return the view with paginated products
            return View(lstSP);
        }

        public IActionResult ProductCate(int? Id)
        {
            // Check if the parameter is null
            if (Id == null)
            {
                return BadRequest();
            }

            // Load products based on the specified criteria
            var lstSP = _context.SanPhams
                .Where(n => n.Maloaisp == Id)
                .Include(n => n.MaloaispNavigation);

            // Check if there are any products
            if (lstSP.Count() == 0)
            {
                return NotFound();
            }
            ViewBag.MaLoaiSP = Id;

            // Return the view with paginated products
            return View(lstSP.OrderBy(n => n.Idsp));
        }
    }
}
