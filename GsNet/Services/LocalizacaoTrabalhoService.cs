using GsNetApi.Data;
using GsNetApi.Models;
using GsNetApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GsNetApi.Services
{
    public class LocalizacaoTrabalhoService : ILocalizacaoTrabalhoService
    {
        private readonly ApplicationDbContext _context;

        public LocalizacaoTrabalhoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LocalizacaoTrabalho>> GetAllAsync()
        {
            return await _context.LocalizacaoTrabalho.Include(l => l.Usuarios).ToListAsync();
        }

        public async Task<LocalizacaoTrabalho?> GetByIdAsync(long id)
        {
            return await _context.LocalizacaoTrabalho.Include(l => l.Usuarios).FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<LocalizacaoTrabalho> CreateAsync(LocalizacaoTrabalho entity)
        {
            _context.LocalizacaoTrabalho.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<LocalizacaoTrabalho?> UpdateAsync(long id, LocalizacaoTrabalho entity)
        {
            var existing = await _context.LocalizacaoTrabalho.FindAsync(id);
            if (existing == null) return null;

            existing.Tipo = entity.Tipo;
            existing.GrausCelcius = entity.GrausCelcius;
            existing.NivelUmidade = entity.NivelUmidade;

            _context.LocalizacaoTrabalho.Update(existing);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var existing = await _context.LocalizacaoTrabalho.FindAsync(id);
            if (existing == null) return false;

            _context.LocalizacaoTrabalho.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
