using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Laptop.Areas.Admin.InterfacesRepositories;
using Laptop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Laptop.Areas.Admin.Controllers
{
    [Area("Admin")]
  
    public class BrandController : Controller
    {
        private readonly IBrandRepository _nhaSXRepository;
        private readonly LaptopContext _context;

        public BrandController(IBrandRepository nhaSXRepository, LaptopContext context)
        {
            _nhaSXRepository = nhaSXRepository;
            _context = context;
        }

        // GET: Admin/NhaSX
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Index()
        {
            var Brands = await _nhaSXRepository.GetAll();
            return View(Brands);
        }

        // GET: Admin/NhaSX/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Brand = await _nhaSXRepository.GetById(id.Value);

            if (Brand == null)
            {
                return NotFound();
            }

            return View(Brand);
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
        public async Task<IActionResult> Create([Bind("BrandId,BrandName")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra trường trống
                if (string.IsNullOrWhiteSpace(brand.BrandName))
                {
                    ModelState.AddModelError("BrandName", "Brand name is required.");
                    return View(brand);
                }

                // Kiểm tra trùng lặp BrandName
                var existingBrand = await _context.Brands.FirstOrDefaultAsync(b => b.BrandName == brand.BrandName);
                if (existingBrand != null)
                {
                    ModelState.AddModelError("BrandName", "Brand name already exists.");
                    return View(brand);
                }

                // Thêm Brand vào cơ sở dữ liệu nếu không có lỗi
                await _nhaSXRepository.Create(brand);
                return RedirectToAction(nameof(Index));
            }

            return View(brand);
        }

        [Authorize(Roles = "Manager")]
        // GET: Admin/NhaSX/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Brand = await _nhaSXRepository.GetById(id.Value);

            if (Brand == null)
            {
                return NotFound();
            }

            return View(Brand);
        }
        [Authorize(Roles = "Manager")]
        // POST: Admin/NhaSX/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BrandId,BrandName,Address,Email")] Brand Brand)
        {
            if (id != Brand.BrandId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _nhaSXRepository.Update(Brand);
                return RedirectToAction(nameof(Index));
            }

            return View(Brand);
        }
        [Authorize(Roles = "Manager")]
        // GET: Admin/NhaSX/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Brand = await _nhaSXRepository.GetById(id.Value);

            if (Brand == null)
            {
                return NotFound();
            }

            return View(Brand);
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

        private bool BrandExists(int id)
        {
            return _nhaSXRepository.BrandExists(id);
        }
    }
}
