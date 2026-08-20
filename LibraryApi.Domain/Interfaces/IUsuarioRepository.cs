using LibraryApi.Domain.Entities;

namespace LibraryApi.Domain.Interfaces;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<bool> ExistsByEmailAsync(string email, int? ignoreId = null, CancellationToken cancellationToken = default);
}
