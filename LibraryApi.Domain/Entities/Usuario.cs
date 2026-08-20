namespace LibraryApi.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public DateOnly DataCadastro { get; set; }

    public ICollection<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
