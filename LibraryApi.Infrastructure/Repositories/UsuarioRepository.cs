using LibraryApi.Domain.Entities;
using LibraryApi.Domain.Interfaces;
using LibraryApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Infrastructure.Repositories;

public class UsuarioRepository(LibraryDbContext context) : Repository<Usuario>(context), IUsuarioRepository
{
    public async Task<bool> ExistsByEmailAsync(string email, int? ignoreId = null, CancellationToken cancellationToken = default)
    {
        var query = Context.Usuarios.AsQueryable().Where(x => x.Email.ToLower() == email.ToLower());
        if (ignoreId.HasValue)
        {
            query = query.Where(x => x.Id != ignoreId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }
}
