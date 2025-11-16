using GsNetApi.Models;

namespace GsNetApi.Services.Interfaces
{
    public interface IMensagemService
    {
        Task<IEnumerable<Mensagem>> GetAllAsync();
        Task<Mensagem?> GetByIdAsync(long id);
        Task<Mensagem> CreateAsync(Mensagem entity);
        Task<Mensagem?> UpdateAsync(long id, Mensagem entity);
        Task<bool> DeleteAsync(long id);
    }
}
