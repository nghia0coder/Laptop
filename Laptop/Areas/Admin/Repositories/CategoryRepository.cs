using Laptop.Areas.Admin.InterfacesRepositories;
using Laptop.Models;
using Microsoft.EntityFrameworkCore;

namespace Laptop.Areas.Admin.Repositories
{
    public class CategoryRepository : ICategoryRepositorycs
    {
        private readonly LaptopContext _context;

        public CategoryRepository(LaptopContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(m => m.CategoryId == id);
        }

        public async Task CreateAsync(Category Category)
        {
            _context.Add(Category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category Category)
        {
            _context.Update(Category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var Category = await _context.Categories.FindAsync(id);
            if (Category != null)
            {
                _context.Categories.Remove(Category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Categories.AnyAsync(e => e.CategoryId == id);
        }
    }
}
