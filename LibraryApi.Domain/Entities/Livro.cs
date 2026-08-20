namespace LibraryApi.Domain.Entities;

public class Livro
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int AnoPublicacao { get; set; }
    public int QuantidadeTotal { get; set; }
    public int QuantidadeDisponivel { get; set; }

    public int AutorId { get; set; }
    public int CategoriaId { get; set; }

    public Autor? Autor { get; set; }
    public Categoria? Categoria { get; set; }

    public ICollection<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
