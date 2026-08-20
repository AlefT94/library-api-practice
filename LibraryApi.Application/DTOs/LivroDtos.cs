using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Application.DTOs;

public record LivroResponseDto(
    int Id,
    string Titulo,
    string ISBN,
    int AnoPublicacao,
    int QuantidadeTotal,
    int QuantidadeDisponivel,
    int AutorId,
    int CategoriaId);

public class CreateLivroDto
{
    [Required, MaxLength(300)]
    public string Titulo { get; set; } = string.Empty;

    [Required, MaxLength(30)]
    public string ISBN { get; set; } = string.Empty;

    [Range(1, 3000)]
    public int AnoPublicacao { get; set; }

    [Range(1, int.MaxValue)]
    public int QuantidadeTotal { get; set; }

    [Range(0, int.MaxValue)]
    public int QuantidadeDisponivel { get; set; }

    [Range(1, int.MaxValue)]
    public int AutorId { get; set; }

    [Range(1, int.MaxValue)]
    public int CategoriaId { get; set; }
}

public class UpdateLivroDto : CreateLivroDto
{
}
