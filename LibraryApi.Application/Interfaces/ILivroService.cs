using LibraryApi.Application.DTOs;

namespace LibraryApi.Application.Interfaces;

public interface ILivroService
{
    Task<IEnumerable<LivroResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<LivroResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<LivroResponseDto> CreateAsync(CreateLivroDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, UpdateLivroDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
