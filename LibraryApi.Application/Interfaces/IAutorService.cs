using LibraryApi.Application.DTOs;

namespace LibraryApi.Application.Interfaces;

public interface IAutorService
{
    Task<IEnumerable<AutorResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AutorResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<AutorResponseDto> CreateAsync(CreateAutorDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, UpdateAutorDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
