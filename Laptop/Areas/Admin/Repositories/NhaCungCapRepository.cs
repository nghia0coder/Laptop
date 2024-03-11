using GiayDep.Areas.Admin.InterfacesRepositories;
using Laptop.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiayDep.Areas.Admin.Repositories
{
    public class NhaCCRepository : INhaCungCapRepositorycs
    {
        private readonly LaptopContext _context;

        public NhaCCRepository(LaptopContext context)
        {
            _context = context;
        }

        public async Task<List<NhaCungCap>> GetAll()
        {
            return await _context.NhaCungCaps.Include(n => n.IdnhasxNavigation).ToListAsync();
        }

        public async Task<NhaCungCap> GetById(int id)
        {
            return await _context.NhaCungCaps.Include(n => n.IdnhasxNavigation).FirstOrDefaultAsync(m => m.Idnhacc == id);
        }

        public async Task Create(NhaCungCap nhaCungCap)
        {
            _context.Add(nhaCungCap);
            await _context.SaveChangesAsync();
        }

        public async Task Update(NhaCungCap nhaCungCap)
        {
            _context.Update(nhaCungCap);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var nhaCungCap = await GetById(id);
            if (nhaCungCap != null)
            {
                _context.NhaCungCaps.Remove(nhaCungCap);
                await _context.SaveChangesAsync();
            }
        }

        public bool NhaCungCapExists(int id)
        {
            return _context.NhaCungCaps.Any(e => e.Idnhacc == id);
        }
    }
}
