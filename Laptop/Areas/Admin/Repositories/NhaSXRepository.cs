using GiayDep.Areas.Admin.InterfacesRepositories;
using Laptop.Models;
using Microsoft.EntityFrameworkCore;

namespace GiayDep.Areas.Admin.Repositories
{
    public class NhaSXRepository : INhaSXRepository
    {
        private readonly LaptopContext _context;

        public NhaSXRepository(LaptopContext context)
        {
            _context = context;
        }

        public async Task<List<NhaSanXuat>> GetAll()
        {
            return await _context.NhaSanXuats.ToListAsync();
        }

        public async Task<NhaSanXuat> GetById(int id)
        {
            return await _context.NhaSanXuats.FirstOrDefaultAsync(m => m.Idnhasx == id);
        }

        public async Task Create(NhaSanXuat nhaSanXuat)
        {
            _context.Add(nhaSanXuat);
            await _context.SaveChangesAsync();
        }

        public async Task Update(NhaSanXuat nhaSanXuat)
        {
            _context.Update(nhaSanXuat);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var nhaSanXuat = await GetById(id);
            if (nhaSanXuat != null)
            {
                _context.NhaSanXuats.Remove(nhaSanXuat);
                await _context.SaveChangesAsync();
            }
        }


        public bool NhaSanXuatExists(int id)
        {
            return _context.NhaSanXuats.Any(e => e.Idnhasx == id);
        }
     

    }
}
