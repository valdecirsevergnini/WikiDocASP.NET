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
                .HasForeignKey(t => t.ModuloId) // Define a chave estrangeira
                .OnDelete(DeleteBehavior.Cascade); // Configura exclusão em cascata

            // Configuração adicional para o Usuario
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Senha)
                .IsRequired();

            // Adicionar dados iniciais
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    Username = "admin",
                    Senha = "admin123", // Para produção, use hashing
                    Nome = "Administrador do Sistema",
                    IsAdmin = true
                }
            );

            modelBuilder.Entity<Modulo>().HasData(
                new Modulo { Id = 1, Nome = "Introdução ao ASP.NET", Descricao = "Módulo básico de introdução ao ASP.NET." },
                new Modulo { Id = 2, Nome = "Entity Framework Core", Descricao = "Módulo sobre EF Core." }
            );

            modelBuilder.Entity<Topico>().HasData(
                new Topico { Id = 1, Titulo = "O que é ASP.NET?", Conteudo = "Introdução ao ASP.NET.", ModuloId = 1 },
                new Topico { Id = 2, Titulo = "Instalando o Entity Framework Core", Conteudo = "Passos para instalação.", ModuloId = 2 }
            );
        }
    }
}
