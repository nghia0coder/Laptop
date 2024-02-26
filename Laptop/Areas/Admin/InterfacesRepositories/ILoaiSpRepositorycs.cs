using GiayDep.Models;

namespace GiayDep.Areas.Admin.InterfacesRepositories
{
    public interface ILoaiSpRepositorycs
    {
        Task<List<LoaiSp>> GetAllAsync();
        Task<LoaiSp> GetByIdAsync(int id);
        Task CreateAsync(LoaiSp loaiSp);
        Task UpdateAsync(LoaiSp loaiSp);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
