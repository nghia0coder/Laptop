using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GiayDep.Models;
using GiayDep.Areas.Admin.InterfacesRepositories;
using Laptop.Models;

namespace GiayDep.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoaiSPController : Controller
    {
        private readonly LaptopContext _context;

        public LoaiSPController(LaptopContext context)
        {
            _context = context;
            
        }

        // GET: Admin/LoaiSP
        public async Task<IActionResult> Index(List<LoaiSp>? loaiSps, int pg=1 )
        {
            const int pageSize = 2;
            if (pg < 1)
                pg = 1;

            int recsCount = _context.LoaiSps.Count();

            var pager = new Pager(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            var data = _context.LoaiSps.Skip(recSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;

            //var nhaSanXuats = await _context.NhaSanXuats.GetAll();
            //return View(nhaSanXuats);

            return View(data);
        }

        // GET: Admin/LoaiSP/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var loaiSp = await _context.LoaiSps.FindAsync(id);
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
            
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
           
        }

        // GET: Admin/LoaiSP/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var loaiSp = await _context.LoaiSps.FindAsync(id);
            if (loaiSp == null)
            {
                return NotFound();
            }
            return View(loaiSp);
        }

        // POST: Admin/LoaiSP/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  LoaiSp loaiSp)
        {
            if (id != loaiSp.Idloai)
            {
                return NotFound();
            }
                try
                {
                    _context.LoaiSps.Update(loaiSp);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiSpExists(loaiSp.Idloai))
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

        // GET: Admin/LoaiSP/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var loaiSp = await _context.LoaiSps.FindAsync(id);
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
            var loaiSp = await _context.LoaiSps.FindAsync(id);
            if (loaiSp == null)
            {
                return NotFound(); // Return 404 if the record doesn't exist
            }

            _context.LoaiSps.Remove(loaiSp); // Remove the record
            await _context.SaveChangesAsync(); // Save changes to the database

            return RedirectToAction(nameof(Index)); // Redirect to the index action
        }


        private bool LoaiSpExists(int id)
        {
            return (_context.LoaiSps?.Any(e => e.Idloai == id)).GetValueOrDefault();
        }
    }
}
