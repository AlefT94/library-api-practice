using LibraryApi.Domain.Enums;

namespace LibraryApi.Domain.Entities;

public class Emprestimo
{
    public int Id { get; set; }

    public int LivroId { get; set; }
    public int UsuarioId { get; set; }

    public DateOnly DataEmprestimo { get; set; }
    public DateOnly DataDevolucaoPrevista { get; set; }
    public DateOnly? DataDevolucaoReal { get; set; }

    public EmprestimoStatus Status { get; set; } = EmprestimoStatus.Ativo;

    public Livro? Livro { get; set; }
    public Usuario? Usuario { get; set; }
}
