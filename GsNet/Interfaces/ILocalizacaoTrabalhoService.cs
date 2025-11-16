using GsNetApi.Models;

namespace GsNetApi.Services.Interfaces
{
    public interface ILocalizacaoTrabalhoService
    {
        Task<IEnumerable<LocalizacaoTrabalho>> GetAllAsync();
        Task<LocalizacaoTrabalho?> GetByIdAsync(long id);
        Task<LocalizacaoTrabalho> CreateAsync(LocalizacaoTrabalho entity);
        Task<LocalizacaoTrabalho?> UpdateAsync(long id, LocalizacaoTrabalho entity);
        Task<bool> DeleteAsync(long id);
    }
}
