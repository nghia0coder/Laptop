using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GiayDep.Areas.Admin.InterfacesRepositories;
using GiayDep.Models;
using Microsoft.AspNetCore.Authorization;

namespace GiayDep.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NhaCCController : Controller
    {
        private readonly INhaCungCapRepositorycs _nhaCCRepository;
        private readonly INhaSXRepository nhaSXRepository;

        public NhaCCController(INhaCungCapRepositorycs nhaCCRepository, INhaSXRepository nhaSXRepository)
        {
            _nhaCCRepository = nhaCCRepository;
            this.nhaSXRepository = nhaSXRepository;
        }
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Index()
        {
            var nhaCungCaps = await _nhaCCRepository.GetAll();
            return View(nhaCungCaps);
        }

        // GET: Admin/NhaCC/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhaCungCap = await _nhaCCRepository.GetById(id.Value);

            if (nhaCungCap == null)
            {
                return NotFound();
            }

            return View(nhaCungCap);
        }
        [Authorize(Roles = "Manager")]
        // GET: Admin/NhaCC/Create
        public async Task<IActionResult> Create()
        {   var sxList = await nhaSXRepository.GetAll();
            ViewData["Idnhasx"] = new SelectList(sxList, "Idnhasx", "Tennhasx");
            return View();
        }
        [Authorize(Roles = "Manager")]
        // POST: Admin/NhaCC/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idnhacc,Tennhacc,Diachi,Sdt,Email,Idnhasx")] NhaCungCap nhaCungCap)
        {
            
                await _nhaCCRepository.Create(nhaCungCap);
                return RedirectToAction(nameof(Index));
          
        }
        [Authorize(Roles = "Manager")]
        // GET: Admin/NhaCC/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhaCungCap = await _nhaCCRepository.GetById(id.Value);

            if (nhaCungCap == null)
            {
                return NotFound();
            }
            var sxList = await nhaSXRepository.GetAll();
            ViewData["Idnhasx"] = new SelectList(sxList, "Idnhasx", "Idnhasx");
            return View(nhaCungCap);
        }
        [Authorize(Roles = "Manager")]
        // POST: Admin/NhaCC/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idnhacc,Tennhacc,Diachi,Sdt,Email,Idnhasx")] NhaCungCap nhaCungCap)
        {
            if (id != nhaCungCap.Idnhacc)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                await _nhaCCRepository.Update(nhaCungCap);
                return RedirectToAction(nameof(Index));
            }
            var sxList = await nhaSXRepository.GetAll();
            ViewData["Idnhasx"] = new SelectList(sxList, "Idnhasx", "Idnhasx");
            return View(nhaCungCap);
        }
        [Authorize(Roles = "Manager")]
        // GET: Admin/NhaCC/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhaCungCap = await _nhaCCRepository.GetById(id.Value);

            if (nhaCungCap == null)
            {
                return NotFound();
            }

            return View(nhaCungCap);
        }
        [Authorize(Roles = "Manager")]
        // POST: Admin/NhaCC/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _nhaCCRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool NhaCungCapExists(int id)
        {
            return _nhaCCRepository.NhaCungCapExists(id);
        }
    }
}
