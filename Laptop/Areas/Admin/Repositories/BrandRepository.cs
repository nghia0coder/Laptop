using Laptop.Areas.Admin.InterfacesRepositories;
using Laptop.Models;
using Microsoft.EntityFrameworkCore;

namespace Laptop.Areas.Admin.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly LaptopContext _context;

        public BrandRepository(LaptopContext context)
        {
            _context = context;
        }

        public async Task<List<Brand>> GetAll()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<Brand> GetById(int id)
        {
            return await _context.Brands.FirstOrDefaultAsync(m => m.BrandId == id);
        }

        public async Task Create(Brand Brand)
        {
            _context.Add(Brand);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Brand Brand)
        {
            _context.Update(Brand);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var Brand = await GetById(id);
            if (Brand != null)
            {
                _context.Brands.Remove(Brand);
                await _context.SaveChangesAsync();
            }
        }


        public bool BrandExists(int id)
        {
            return _context.Brands.Any(e => e.BrandId == id);
        }
     

    }
}
