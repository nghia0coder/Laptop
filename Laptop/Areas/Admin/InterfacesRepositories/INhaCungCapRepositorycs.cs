using GiayDep.Models;

namespace GiayDep.Areas.Admin.InterfacesRepositories
{
    public interface INhaCungCapRepositorycs
    {
        Task<List<NhaCungCap>> GetAll();
        Task<NhaCungCap> GetById(int id);
        Task Create(NhaCungCap nhaCungCap);
        Task Update(NhaCungCap nhaCungCap);
        Task Delete(int id);
        bool NhaCungCapExists(int id);
    }
}
