using Laptop.Models;

namespace GiayDep.Areas.Admin.InterfacesRepositories
{
    public interface INhaSXRepository
    {
        Task<List<NhaSanXuat>> GetAll();
        Task<NhaSanXuat> GetById(int id);
        Task Create(NhaSanXuat nhaSanXuat);
        Task Update(NhaSanXuat nhaSanXuat);
        Task Delete(int id);

        bool NhaSanXuatExists(int id);

    }
}
