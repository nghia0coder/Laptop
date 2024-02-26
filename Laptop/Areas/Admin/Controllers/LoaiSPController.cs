using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GiayDep.Models;
using GiayDep.Areas.Admin.InterfacesRepositories;

namespace GiayDep.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoaiSPController : Controller
    {
        private readonly ILoaiSpRepositorycs _repository;

        public LoaiSPController(ILoaiSpRepositorycs repository)
        {
            _repository = repository;
        }

        // GET: Admin/LoaiSP
        public async Task<IActionResult> Index()
        {
            var loaiSps = await _repository.GetAllAsync();
            return View(loaiSps);
        }

        // GET: Admin/LoaiSP/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var loaiSp = await _repository.GetByIdAsync(id);
            if (loaiSp == null)
            {
                return NotFound();
            }

            return View(loaiSp);
        }

        // GET: Admin/LoaiSP/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/LoaiSP/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Tenloai")] LoaiSp loaiSp)
        {
            
                await _repository.CreateAsync(loaiSp);
                return RedirectToAction(nameof(Index));
           
        }

        // GET: Admin/LoaiSP/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var loaiSp = await _repository.GetByIdAsync(id);
            if (loaiSp == null)
            {
                return NotFound();
            }
            return View(loaiSp);
        }

        // POST: Admin/LoaiSP/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Tenloai")] LoaiSp loaiSp)
        {
            if (id != loaiSp.Idloai)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.UpdateAsync(loaiSp);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_repository.ExistsAsync(loaiSp.Idloai).Result)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(loaiSp);
        }

        // GET: Admin/LoaiSP/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var loaiSp = await _repository.GetByIdAsync(id);
            if (loaiSp == null)
            {
                return NotFound();
            }

            return View(loaiSp);
        }

        // POST: Admin/LoaiSP/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool LoaiSpExists(int id)
        {
            return _repository.ExistsAsync(id).Result;
        }
    }
}
