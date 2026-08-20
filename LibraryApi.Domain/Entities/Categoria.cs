namespace LibraryApi.Domain.Entities;

public class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }

    public ICollection<Livro> Livros { get; set; } = new List<Livro>();
}
