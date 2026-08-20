using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Application.DTOs;

public record CategoriaResponseDto(int Id, string Nome, string? Descricao);

public class CreateCategoriaDto
{
    [Required, MaxLength(120)]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Descricao { get; set; }
}

public class UpdateCategoriaDto : CreateCategoriaDto
{
}
