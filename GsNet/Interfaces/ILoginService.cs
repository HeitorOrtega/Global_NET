using GsNetApi.Models;

namespace GsNetApi.Services.Interfaces
{
    public interface ILoginService
    {
        Task<IEnumerable<Login>> GetAllAsync();
        Task<Login?> GetByIdAsync(long id);
        Task<Login> CreateAsync(Login entity);
        Task<Login?> UpdateAsync(long id, Login entity);
        Task<bool> DeleteAsync(long id);
    }
}
