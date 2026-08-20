using LibraryApi.Application.DTOs;

namespace LibraryApi.Application.Interfaces;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<UsuarioResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<UsuarioResponseDto> CreateAsync(CreateUsuarioDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, UpdateUsuarioDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
