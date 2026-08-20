using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Application.DTOs;

public record AutorResponseDto(int Id, string Nome, string? Biografia, string? Pais);

public class CreateAutorDto
{
    [Required, MaxLength(150)]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Biografia { get; set; }

    [MaxLength(100)]
    public string? Pais { get; set; }
}

public class UpdateAutorDto : CreateAutorDto
{
}
