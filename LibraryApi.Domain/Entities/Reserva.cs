using LibraryApi.Domain.Enums;

namespace LibraryApi.Domain.Entities;

public class Reserva
{
    public int Id { get; set; }

    public int LivroId { get; set; }
    public int UsuarioId { get; set; }

    public DateOnly DataReserva { get; set; }
    public ReservaStatus Status { get; set; } = ReservaStatus.Ativa;

    public Livro? Livro { get; set; }
    public Usuario? Usuario { get; set; }
}
