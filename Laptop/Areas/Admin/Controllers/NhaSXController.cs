using GiayDep.Areas.Admin.InterfacesRepositories;
using GiayDep.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiayDep.Areas.Admin.Controllers
{
    [Area("Admin")]
  
    public class NhaSXController : Controller
    {
        private readonly INhaSXRepository _nhaSXRepository;

        public NhaSXController(INhaSXRepository nhaSXRepository)
        {
            _nhaSXRepository = nhaSXRepository;
        }

        // GET: Admin/NhaSX
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Index()
        {
            var nhaSanXuats = await _nhaSXRepository.GetAll();
            return View(nhaSanXuats);
        }

        // GET: Admin/NhaSX/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhaSanXuat = await _nhaSXRepository.GetById(id.Value);

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
                await _nhaSXRepository.Create(nhaSanXuat);
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

            var nhaSanXuat = await _nhaSXRepository.GetById(id.Value);

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
                await _nhaSXRepository.Update(nhaSanXuat);
                return RedirectToAction(nameof(Index));
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

            var nhaSanXuat = await _nhaSXRepository.GetById(id.Value);

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
            await _nhaSXRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool NhaSanXuatExists(int id)
        {
            return _nhaSXRepository.NhaSanXuatExists(id);
        }
    }
}
