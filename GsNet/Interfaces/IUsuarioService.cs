using GsNetApi.Models;

namespace GsNetApi.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(long id);
        Task<Usuario> CreateAsync(Usuario usuario);
        Task<Usuario?> UpdateAsync(long id, Usuario usuario);
        Task<bool> DeleteAsync(long id);
    }
}
