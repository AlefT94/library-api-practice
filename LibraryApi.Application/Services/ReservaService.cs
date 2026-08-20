using LibraryApi.Application.DTOs;
using LibraryApi.Application.Interfaces;
using LibraryApi.Domain.Entities;
using LibraryApi.Domain.Enums;
using LibraryApi.Domain.Interfaces;

namespace LibraryApi.Application.Services;

public class ReservaService(IUnitOfWork unitOfWork) : IReservaService
{
    public async Task<IEnumerable<ReservaResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var reservas = await unitOfWork.Reservas.GetAllAsync(cancellationToken);
        return reservas.Select(Map);
    }

    public async Task<ReservaResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var reserva = await unitOfWork.Reservas.GetByIdAsync(id, cancellationToken);
        return reserva is null ? null : Map(reserva);
    }

    public async Task<ReservaResponseDto> CreateAsync(CreateReservaDto dto, CancellationToken cancellationToken = default)
    {
        _ = await unitOfWork.Livros.GetByIdAsync(dto.LivroId, cancellationToken)
            ?? throw new InvalidOperationException("Livro informado não existe.");

        _ = await unitOfWork.Usuarios.GetByIdAsync(dto.UsuarioId, cancellationToken)
            ?? throw new InvalidOperationException("Usuário informado não existe.");

        var reserva = new Reserva
        {
            LivroId = dto.LivroId,
            UsuarioId = dto.UsuarioId,
            DataReserva = dto.DataReserva ?? DateOnly.FromDateTime(DateTime.UtcNow.Date),
            Status = ReservaStatus.Ativa
        };

        await unitOfWork.Reservas.AddAsync(reserva, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Map(reserva);
    }

    public async Task<bool> UpdateAsync(int id, UpdateReservaDto dto, CancellationToken cancellationToken = default)
    {
        var reserva = await unitOfWork.Reservas.GetByIdAsync(id, cancellationToken);
        if (reserva is null)
        {
            return false;
        }

        _ = await unitOfWork.Livros.GetByIdAsync(dto.LivroId, cancellationToken)
            ?? throw new InvalidOperationException("Livro informado não existe.");

        _ = await unitOfWork.Usuarios.GetByIdAsync(dto.UsuarioId, cancellationToken)
            ?? throw new InvalidOperationException("Usuário informado não existe.");

        reserva.LivroId = dto.LivroId;
        reserva.UsuarioId = dto.UsuarioId;
        reserva.DataReserva = dto.DataReserva;
        reserva.Status = dto.Status;

        unitOfWork.Reservas.Update(reserva);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var reserva = await unitOfWork.Reservas.GetByIdAsync(id, cancellationToken);
        if (reserva is null)
        {
            return false;
        }

        unitOfWork.Reservas.Delete(reserva);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static ReservaResponseDto Map(Reserva reserva) =>
        new(reserva.Id, reserva.LivroId, reserva.UsuarioId, reserva.DataReserva, reserva.Status);
}
