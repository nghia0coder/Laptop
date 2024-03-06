using GiayDep.Areas.Admin.InterfacesRepositories;
using Laptop.Models;
using Microsoft.EntityFrameworkCore;

namespace GiayDep.Areas.Admin.Repositories
{
    public class LoaiSpRepository : ILoaiSpRepositorycs
    {
        private readonly LaptopContext _context;

        public LoaiSpRepository(LaptopContext context)
        {
            _context = context;
        }

        public async Task<List<LoaiSp>> GetAllAsync()
        {
            return await _context.LoaiSps.ToListAsync();
        }

        public async Task<LoaiSp> GetByIdAsync(int id)
        {
            return await _context.LoaiSps.FirstOrDefaultAsync(m => m.Idloai == id);
        }

        public async Task CreateAsync(LoaiSp loaiSp)
        {
            _context.Add(loaiSp);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LoaiSp loaiSp)
        {
            _context.Update(loaiSp);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var loaiSp = await _context.LoaiSps.FindAsync(id);
            if (loaiSp != null)
            {
                _context.LoaiSps.Remove(loaiSp);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.LoaiSps.AnyAsync(e => e.Idloai == id);
        }
    }
}
