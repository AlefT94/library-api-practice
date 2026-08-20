using System.ComponentModel.DataAnnotations;
using LibraryApi.Domain.Enums;

namespace LibraryApi.Application.DTOs;

public record EmprestimoResponseDto(
    int Id,
    int LivroId,
    int UsuarioId,
    DateOnly DataEmprestimo,
    DateOnly DataDevolucaoPrevista,
    DateOnly? DataDevolucaoReal,
    EmprestimoStatus Status);

public class CreateEmprestimoDto
{
    [Range(1, int.MaxValue)]
    public int LivroId { get; set; }

    [Range(1, int.MaxValue)]
    public int UsuarioId { get; set; }

    public DateOnly? DataEmprestimo { get; set; }

    [Required]
    public DateOnly DataDevolucaoPrevista { get; set; }
}

public class UpdateEmprestimoDto
{
    [Range(1, int.MaxValue)]
    public int LivroId { get; set; }

    [Range(1, int.MaxValue)]
    public int UsuarioId { get; set; }

    [Required]
    public DateOnly DataEmprestimo { get; set; }

    [Required]
    public DateOnly DataDevolucaoPrevista { get; set; }

    public DateOnly? DataDevolucaoReal { get; set; }

    [Required]
    public EmprestimoStatus Status { get; set; }
}
