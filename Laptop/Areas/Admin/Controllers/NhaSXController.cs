using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GiayDep.Areas.Admin.InterfacesRepositories;
using GiayDep.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Laptop.Models;

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
                _context.Add(nhaSanXuat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof (Index));
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
