using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GiayDep.Areas.Admin.InterfacesRepositories;
using GiayDep.Models;
using Microsoft.AspNetCore.Authorization;

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
                await _nhaSXRepository.Create(nhaSanXuat);
                return RedirectToAction(nameof(Index));
            }

            return View(nhaSanXuat);
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
