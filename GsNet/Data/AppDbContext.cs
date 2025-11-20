using Microsoft.EntityFrameworkCore;
using GsNetApi.Models;

namespace GsNetApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) 
        { 
        }

        public DbSet<LocalizacaoTrabalho> LocalizacaoTrabalho { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Mensagem> Mensagem { get; set; }
        public DbSet<Login> Login { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("RM557825");

            modelBuilder.Entity<LocalizacaoTrabalho>().ToTable("TB_GS_LOCALIZACAO");
            modelBuilder.Entity<Usuario>().ToTable("TB_GS_USUARIO");
            modelBuilder.Entity<Mensagem>().ToTable("TB_GS_MENSAGEM");
            modelBuilder.Entity<Login>().ToTable("TB_GS_LOGIN");
            
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.LocTrabalho)           
                .WithMany(lt => lt.Usuarios)        
                .HasForeignKey(u => u.LocalizacaoTrabalhoId) 
                .HasConstraintName("FK_USUARIO_LOCALIZACAO")
                .OnDelete(DeleteBehavior.Restrict); 
            
            modelBuilder.Entity<Mensagem>()
                .HasOne(m => m.Usuario)              
                .WithMany(u => u.Mensagens)           
                .HasForeignKey(m => m.UsuarioId)      
                .HasConstraintName("FK_MENSAGEM_USUARIO")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LocalizacaoTrabalho>()
                .Property(l => l.Tipo);

            base.OnModelCreating(modelBuilder);
        }
    }
}
