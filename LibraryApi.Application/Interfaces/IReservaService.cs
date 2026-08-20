using LibraryApi.Application.DTOs;

namespace LibraryApi.Application.Interfaces;

public interface IReservaService
{
    Task<IEnumerable<ReservaResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ReservaResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ReservaResponseDto> CreateAsync(CreateReservaDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, UpdateReservaDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
