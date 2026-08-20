using LibraryApi.Domain.Entities;
using LibraryApi.Domain.Interfaces;
using LibraryApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Infrastructure.Repositories;

public class LivroRepository(LibraryDbContext context) : Repository<Livro>(context), ILivroRepository
{
    public async Task<bool> ExistsByIsbnAsync(string isbn, int? ignoreId = null, CancellationToken cancellationToken = default)
    {
        var query = Context.Livros.AsQueryable().Where(x => x.ISBN.ToLower() == isbn.ToLower());
        if (ignoreId.HasValue)
        {
            query = query.Where(x => x.Id != ignoreId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }
}
