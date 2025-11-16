using GsNetApi.Services.Interfaces;
using GsNetApi.Data;
using GsNetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GsNetApi.Services
{
    public class LoginService : ILoginService
    {
        private readonly ApplicationDbContext _context;

        public LoginService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Login>> GetAllAsync()
        {
            return await _context.Login.ToListAsync();
        }

        public async Task<Login?> GetByIdAsync(long id)
        {
            return await _context.Login.FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Login> CreateAsync(Login entity)
        {
            _context.Login.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Login?> UpdateAsync(long id, Login entity)
        {
            var existing = await _context.Login.FindAsync(id);
            if (existing == null) return null;

            existing.Email = entity.Email;
            existing.Senha = entity.Senha;

            _context.Login.Update(existing);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var existing = await _context.Login.FindAsync(id);
            if (existing == null) return false;

            _context.Login.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
