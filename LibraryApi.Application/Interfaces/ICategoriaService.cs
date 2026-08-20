using LibraryApi.Application.DTOs;

namespace LibraryApi.Application.Interfaces;

public interface ICategoriaService
{
    Task<IEnumerable<CategoriaResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CategoriaResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CategoriaResponseDto> CreateAsync(CreateCategoriaDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, UpdateCategoriaDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
