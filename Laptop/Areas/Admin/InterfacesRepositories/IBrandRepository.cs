using Laptop.Models;

namespace Laptop.Areas.Admin.InterfacesRepositories
{
    public interface IBrandRepository
    {
        Task<List<Brand>> GetAll();
        Task<Brand> GetById(int id);
        Task Create(Brand Brand);
        Task Update(Brand Brand);
        Task Delete(int id);

        bool BrandExists(int id);

    }
}
