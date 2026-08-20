using LibraryApi.Domain.Interfaces;
using LibraryApi.Infrastructure.Persistence;

namespace LibraryApi.Infrastructure.Repositories;

public class UnitOfWork(
    LibraryDbContext context,
    IAutorRepository autores,
    ICategoriaRepository categorias,
    ILivroRepository livros,
    IUsuarioRepository usuarios,
    IEmprestimoRepository emprestimos,
    IReservaRepository reservas) : IUnitOfWork
{
    public IAutorRepository Autores { get; } = autores;
    public ICategoriaRepository Categorias { get; } = categorias;
    public ILivroRepository Livros { get; } = livros;
    public IUsuarioRepository Usuarios { get; } = usuarios;
    public IEmprestimoRepository Emprestimos { get; } = emprestimos;
    public IReservaRepository Reservas { get; } = reservas;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => context.SaveChangesAsync(cancellationToken);
}
