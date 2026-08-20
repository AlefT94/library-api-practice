using LibraryApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Infrastructure.Persistence;

public class LibraryDbContext(DbContextOptions<LibraryDbContext> options) : DbContext(options)
{
    public DbSet<Livro> Livros => Set<Livro>();
    public DbSet<Autor> Autores => Set<Autor>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Emprestimo> Emprestimos => Set<Emprestimo>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Reserva> Reservas => Set<Reserva>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Livro>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Titulo).IsRequired().HasMaxLength(300);
            entity.Property(x => x.ISBN).IsRequired().HasMaxLength(30);
            entity.HasIndex(x => x.ISBN).IsUnique();

            entity.HasOne(x => x.Autor)
                .WithMany(x => x.Livros)
                .HasForeignKey(x => x.AutorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Categoria)
                .WithMany(x => x.Livros)
                .HasForeignKey(x => x.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Autor>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Nome).IsRequired().HasMaxLength(150);
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Nome).IsRequired().HasMaxLength(120);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Nome).IsRequired().HasMaxLength(150);
            entity.Property(x => x.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(x => x.Email).IsUnique();
        });

        modelBuilder.Entity<Emprestimo>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasOne(x => x.Livro)
                .WithMany(x => x.Emprestimos)
                .HasForeignKey(x => x.LivroId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Usuario)
                .WithMany(x => x.Emprestimos)
                .HasForeignKey(x => x.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasOne(x => x.Livro)
                .WithMany(x => x.Reservas)
                .HasForeignKey(x => x.LivroId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Usuario)
                .WithMany(x => x.Reservas)
                .HasForeignKey(x => x.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
