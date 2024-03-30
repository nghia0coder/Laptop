using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Laptop.Areas.Admin.InterfacesRepositories;
using Laptop.Models;
using Microsoft.AspNetCore.Authorization;
//uuyuyuu


namespace Laptop.Areas.Admin.Controllers
{
    [Area("Admin")]
  
    public class BrandController : Controller
    {
        private readonly IBrandRepository _nhaSXRepository;

        public BrandController(IBrandRepository nhaSXRepository)
        {
            _nhaSXRepository = nhaSXRepository;
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
        public async Task<IActionResult> Create([Bind("BrandId,BrandName,Address,Email")] Brand _Brand)
        {
            if (ModelState.IsValid)
            {
                await _nhaSXRepository.Create(_Brand);
                return RedirectToAction(nameof(Index));
            }

            return View(_Brand);
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
