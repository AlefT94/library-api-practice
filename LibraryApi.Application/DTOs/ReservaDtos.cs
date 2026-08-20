using System.ComponentModel.DataAnnotations;
using LibraryApi.Domain.Enums;

namespace LibraryApi.Application.DTOs;

public record ReservaResponseDto(int Id, int LivroId, int UsuarioId, DateOnly DataReserva, ReservaStatus Status);

public class CreateReservaDto
{
    [Range(1, int.MaxValue)]
    public int LivroId { get; set; }

    [Range(1, int.MaxValue)]
    public int UsuarioId { get; set; }

    public DateOnly? DataReserva { get; set; }
}

public class UpdateReservaDto
{
    [Range(1, int.MaxValue)]
    public int LivroId { get; set; }

    [Range(1, int.MaxValue)]
    public int UsuarioId { get; set; }

    [Required]
    public DateOnly DataReserva { get; set; }

    [Required]
    public ReservaStatus Status { get; set; }
}
