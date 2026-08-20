using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Application.DTOs;

public record UsuarioResponseDto(int Id, string Nome, string Email, string? Telefone, DateOnly DataCadastro);

public class CreateUsuarioDto
{
    [Required, MaxLength(150)]
    public string Nome { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(25)]
    public string? Telefone { get; set; }

    public DateOnly? DataCadastro { get; set; }
}

public class UpdateUsuarioDto
{
    [Required, MaxLength(150)]
    public string Nome { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(25)]
    public string? Telefone { get; set; }

    public DateOnly DataCadastro { get; set; }
}
