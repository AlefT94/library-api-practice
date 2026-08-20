namespace LibraryApi.Domain.Interfaces;

public interface IUnitOfWork
{
    IAutorRepository Autores { get; }
    ICategoriaRepository Categorias { get; }
    ILivroRepository Livros { get; }
    IUsuarioRepository Usuarios { get; }
    IEmprestimoRepository Emprestimos { get; }
    IReservaRepository Reservas { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
