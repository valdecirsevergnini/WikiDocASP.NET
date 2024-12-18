using Microsoft.EntityFrameworkCore;
using WikiSistemaASP.NET.Models;

namespace WikiSistemaASP.NET.Data
{
    public class WikiDbContext : DbContext
    {
        public WikiDbContext(DbContextOptions<WikiDbContext> options)
            : base(options)
        {
        }

        // DbSets para representar as tabelas no banco de dados
        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Modulo> Modulos { get; set; } = null!;
        public DbSet<Topico> Topicos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração de relacionamento entre Modulo e Topico
            modelBuilder.Entity<Topico>()
                .HasOne(t => t.Modulo) // Um Tópico pertence a um Módulo
                .WithMany(m => m.Topicos) // Um Módulo pode ter vários Tópicos
                .HasForeignKey(t => t.ModuloId); // Define a chave estrangeira
            
            // Adicionar dados iniciais para o usuário administrador
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    Username = "admin",
                    Senha = "admin123", // Senha padrão para testes
                    Nome = "Administrador do Sistema",
                    IsAdmin = true
                }
            );
        }
    }
}
