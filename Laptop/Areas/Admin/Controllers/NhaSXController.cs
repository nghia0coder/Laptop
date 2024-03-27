using GiayDep.Areas.Admin.InterfacesRepositories;
using Laptop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiayDep.Areas.Admin.Controllers
{
    [Area("Admin")]
  
    public class NhaSXController : Controller
    {
        private readonly LaptopContext _context;

        public NhaSXController(LaptopContext context)
        {
            _context = context;
        }

        // GET: Admin/NhaSX
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Index(List<NhaSanXuat>? nhaSanXuats, int pg=1)
        {
            const int pageSize = 2;
            if (pg < 1)
                pg = 1;

            int recsCount = _context.NhaSanXuats.Count();

            var pager = new Pager(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            var data = _context.NhaSanXuats.Skip(recSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;

            //var nhaSanXuats = await _context.NhaSanXuats.GetAll();
            //return View(nhaSanXuats);

            return View(data);
        }

        // GET: Admin/NhaSX/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhaSanXuat = await _context.NhaSanXuats.FindAsync(id.Value);

            if (nhaSanXuat == null)
            {
                return NotFound();
            }

            return View(nhaSanXuat);
        }
        [Authorize(Roles = "Manager")]
        // GET: Admin/NhaSX/Create
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Manager")]
        // POST: Admin/NhaSX/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idnhasx,Tennhasx,Diachi,Sđt,Email")] NhaSanXuat nhaSanXuat)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra tên nhà sản xuất có kí tự không
                if (string.IsNullOrWhiteSpace(nhaSanXuat.Tennhasx))
                {
                    ModelState.AddModelError("Tennhasx", "Tên nhà sản xuất không được để trống.");
                    return View(nhaSanXuat);
                }

                // Kiểm tra địa chỉ có kí tự không
                if (string.IsNullOrWhiteSpace(nhaSanXuat.Diachi))
                {
                    ModelState.AddModelError("Diachi", "Địa chỉ không được để trống.");
                    return View(nhaSanXuat);
                }
                // Kiểm tra định dạng số điện thoại
                if (!IsValidPhoneNumber(nhaSanXuat.Sđt))
                {
                    ModelState.AddModelError("Sđt", "Số điện thoại không hợp lệ.");
                    return View(nhaSanXuat);
                }

                // Kiểm tra định dạng email
                if (!IsValidEmail(nhaSanXuat.Email))
                {
                    ModelState.AddModelError("Email", "Email không hợp lệ.");
                    return View(nhaSanXuat);
                }

                // Gọi repository để thêm mới
                 _context.NhaSanXuats.Add(nhaSanXuat);
                return RedirectToAction(nameof(Index));
            }

            return View(nhaSanXuat);
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            
            return phoneNumber.Length >= 10;
        }

        private bool IsValidEmail(string email)
        {
            
            return email.Contains("@");
        }

        [Authorize(Roles = "Manager")]
        // GET: Admin/NhaSX/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhaSanXuat = await _context.NhaSanXuats.FindAsync(id.Value);

            if (nhaSanXuat == null)
            {
                return NotFound();
            }

            return View(nhaSanXuat);
        }
        [Authorize(Roles = "Manager")]
        // POST: Admin/NhaSX/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idnhasx,Tennhasx,Diachi,Sđt,Email")] NhaSanXuat nhaSanXuat)
        {
            if (id != nhaSanXuat.Idnhasx)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
               _context.NhaSanXuats.Update(nhaSanXuat);
                return RedirectToAction(nameof( Index));
            }

            return View(nhaSanXuat);
        }
        [Authorize(Roles = "Manager")]
        // GET: Admin/NhaSX/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhaSanXuat = await _context.NhaSanXuats.FindAsync(id.Value);

            if (nhaSanXuat == null)
            {
                return NotFound();
            }

            return View(nhaSanXuat);
        }
        [Authorize(Roles = "Manager")]
        // POST: Admin/NhaSX/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nhaSanXuat = await _context.NhaSanXuats.FindAsync(id);
            if (nhaSanXuat == null)
            {
                return NotFound();
            }

            _context.NhaSanXuats.Remove(nhaSanXuat);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
