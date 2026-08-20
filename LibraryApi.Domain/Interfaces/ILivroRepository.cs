using LibraryApi.Domain.Entities;

namespace LibraryApi.Domain.Interfaces;

public interface ILivroRepository : IRepository<Livro>
{
    Task<bool> ExistsByIsbnAsync(string isbn, int? ignoreId = null, CancellationToken cancellationToken = default);
}
