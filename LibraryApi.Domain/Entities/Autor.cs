namespace LibraryApi.Domain.Entities;

public class Autor
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Biografia { get; set; }
    public string? Pais { get; set; }

    public ICollection<Livro> Livros { get; set; } = new List<Livro>();
}
