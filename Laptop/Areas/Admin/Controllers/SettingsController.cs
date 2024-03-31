using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laptop.Models;
using Microsoft.Build.Evaluation;

namespace Laptop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingsController : Controller
    {
        private readonly LaptopContext _context;
        private readonly IWebHostEnvironment _webHost;
        public SettingsController(LaptopContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost; 
        }

        // GET: Admin/Settings
        public async Task<IActionResult> Index()
        {
              return _context.Settings != null ? 
                          View(await _context.Settings.ToListAsync()) :
                          Problem("Entity set 'LaptopContext.Settings'  is null.");
        }

        // GET: Admin/Settings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Settings == null)
            {
                return NotFound();
            }

            var setting = await _context.Settings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (setting == null)
            {
                return NotFound();
            }

            return View(setting);
        }

        // GET: Admin/Settings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Settings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Setting setting)
        {
            string uniqueFileName1 = GetProfilePhotoFileName1(setting);
            setting.ImgUrl = uniqueFileName1;
            
                _context.Add(setting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
          
        }

        // GET: Admin/Settings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Settings == null)
            {
                return NotFound();
            }

            var setting = await _context.Settings.FindAsync(id);
            if (setting == null)
            {
                return NotFound();
            }
            return View(setting);
        }

        // POST: Admin/Settings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ImgUrl,Contents")] Setting setting)
        {
            if (id != setting.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(setting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SettingExists(setting.Id))
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
            return View(setting);
        }

        // GET: Admin/Settings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Settings == null)
            {
                return NotFound();
            }

            var setting = await _context.Settings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (setting == null)
            {
                return NotFound();
            }

            return View(setting);
        }

        // POST: Admin/Settings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Settings == null)
            {
                return Problem("Entity set 'LaptopContext.Settings'  is null.");
            }
            var setting = await _context.Settings.FindAsync(id);
            if (setting != null)
            {
                _context.Settings.Remove(setting);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private string GetProfilePhotoFileName1(Setting Product)
        {
            string uniqueFileName = null;

            if (Product.Img != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "img/");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Product.Img.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Product.Img.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


        private bool SettingExists(int id)
        {
          return (_context.Settings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
