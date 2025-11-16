using GsNetApi.Data;
using GsNetApi.Models;
using GsNetApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GsNetApi.Services
{
    public class MensagemService : IMensagemService
    {
        private readonly ApplicationDbContext _context;

        public MensagemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Mensagem>> GetAllAsync()
        {
            return await _context.Mensagem.Include(m => m.Usuario).ToListAsync();
        }

        public async Task<Mensagem?> GetByIdAsync(long id)
        {
            return await _context.Mensagem.Include(m => m.Usuario).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Mensagem> CreateAsync(Mensagem entity)
        {
            _context.Mensagem.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Mensagem?> UpdateAsync(long id, Mensagem entity)
        {
            var existing = await _context.Mensagem.FindAsync(id);
            if (existing == null) return null;

            existing.TextoMensagem = entity.TextoMensagem;
            existing.NivelEstresse = entity.NivelEstresse;
            existing.UsuarioId = entity.UsuarioId;

            _context.Mensagem.Update(existing);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var existing = await _context.Mensagem.FindAsync(id);
            if (existing == null) return false;

            _context.Mensagem.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
