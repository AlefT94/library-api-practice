using LibraryApi.Application.DTOs;

namespace LibraryApi.Application.Interfaces;

public interface IEmprestimoService
{
    Task<IEnumerable<EmprestimoResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<EmprestimoResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<EmprestimoResponseDto> CreateAsync(CreateEmprestimoDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, UpdateEmprestimoDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<EmprestimoResponseDto?> RegistrarDevolucaoAsync(int id, DateOnly? dataDevolucaoReal, CancellationToken cancellationToken = default);
}
