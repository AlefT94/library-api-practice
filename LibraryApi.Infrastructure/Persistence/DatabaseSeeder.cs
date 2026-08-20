using LibraryApi.Domain.Entities;
using LibraryApi.Domain.Enums;

namespace LibraryApi.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static void Seed(LibraryDbContext context)
    {
        // Verifica se já existe dados para não duplicar
        if (context.Autores.Any())
            return;

        // === AUTORES ===
        var autores = new List<Autor>
        {
            new() { Id = 1, Nome = "Machado de Assis", Biografia = "Escritor brasileiro, considerado um dos maiores nomes da literatura nacional", Pais = "Brasil" },
            new() { Id = 2, Nome = "Clarice Lispector", Biografia = "Escritora e jornalista nascida na Ucrânia e naturalizada brasileira", Pais = "Brasil" },
            new() { Id = 3, Nome = "Jorge Amado", Biografia = "Escritor brasileiro modernista, autor de obras como Gabriela, Cravo e Canela", Pais = "Brasil" },
            new() { Id = 4, Nome = "Paulo Coelho", Biografia = "Escritor e letrista brasileiro, autor de O Alquimista", Pais = "Brasil" }
        };
        context.Autores.AddRange(autores);

        // === CATEGORIAS ===
        var categorias = new List<Categoria>
        {
            new() { Id = 1, Nome = "Romance", Descricao = "Obras de ficção narrativa longa" },
            new() { Id = 2, Nome = "Ficção", Descricao = "Literatura de imaginação e criação" },
            new() { Id = 3, Nome = "Literatura Brasileira", Descricao = "Obras de autores brasileiros" },
            new() { Id = 4, Nome = "Autoajuda", Descricao = "Livros de desenvolvimento pessoal e motivação" }
        };
        context.Categorias.AddRange(categorias);

        // === LIVROS ===
        var livros = new List<Livro>
        {
            new() { Id = 1, Titulo = "Dom Casmurro", ISBN = "978-8535911664", AnoPublicacao = 1899, QuantidadeTotal = 5, QuantidadeDisponivel = 3, AutorId = 1, CategoriaId = 1 },
            new() { Id = 2, Titulo = "Memórias Póstumas de Brás Cubas", ISBN = "978-8535911671", AnoPublicacao = 1881, QuantidadeTotal = 4, QuantidadeDisponivel = 4, AutorId = 1, CategoriaId = 3 },
            new() { Id = 3, Titulo = "A Hora da Estrela", ISBN = "978-8520925683", AnoPublicacao = 1977, QuantidadeTotal = 3, QuantidadeDisponivel = 2, AutorId = 2, CategoriaId = 2 },
            new() { Id = 4, Titulo = "A Paixão Segundo G.H.", ISBN = "978-8520937235", AnoPublicacao = 1964, QuantidadeTotal = 3, QuantidadeDisponivel = 3, AutorId = 2, CategoriaId = 2 },
            new() { Id = 5, Titulo = "Capitães da Areia", ISBN = "978-8535914063", AnoPublicacao = 1937, QuantidadeTotal = 6, QuantidadeDisponivel = 5, AutorId = 3, CategoriaId = 3 },
            new() { Id = 6, Titulo = "Gabriela, Cravo e Canela", ISBN = "978-8535914070", AnoPublicacao = 1958, QuantidadeTotal = 4, QuantidadeDisponivel = 3, AutorId = 3, CategoriaId = 1 },
            new() { Id = 7, Titulo = "O Alquimista", ISBN = "978-8573022551", AnoPublicacao = 1988, QuantidadeTotal = 10, QuantidadeDisponivel = 8, AutorId = 4, CategoriaId = 4 },
            new() { Id = 8, Titulo = "Brida", ISBN = "978-8573025682", AnoPublicacao = 1990, QuantidadeTotal = 3, QuantidadeDisponivel = 3, AutorId = 4, CategoriaId = 2 }
        };
        context.Livros.AddRange(livros);

        // === USUÁRIOS ===
        var usuarios = new List<Usuario>
        {
            new() { Id = 1, Nome = "Maria Silva", Email = "maria.silva@email.com", Telefone = "(11) 98765-4321", DataCadastro = new DateTime(2024, 1, 15) },
            new() { Id = 2, Nome = "João Santos", Email = "joao.santos@email.com", Telefone = "(21) 97654-3210", DataCadastro = new DateTime(2024, 2, 20) },
            new() { Id = 3, Nome = "Ana Oliveira", Email = "ana.oliveira@email.com", Telefone = "(31) 96543-2109", DataCadastro = new DateTime(2024, 3, 10) },
            new() { Id = 4, Nome = "Carlos Pereira", Email = "carlos.pereira@email.com", Telefone = "(41) 95432-1098", DataCadastro = new DateTime(2024, 4, 5) }
        };
        context.Usuarios.AddRange(usuarios);

        // === EMPRÉSTIMOS ===
        var emprestimos = new List<Emprestimo>
        {
            // Empréstimo ativo - Maria pegou Dom Casmurro
            new() 
            { 
                Id = 1, 
                LivroId = 1, 
                UsuarioId = 1, 
                DataEmprestimo = DateTime.Now.AddDays(-5), 
                DataDevolucaoPrevista = DateTime.Now.AddDays(9), 
                DataDevolucaoReal = null, 
                Status = EmprestimoStatus.Ativo 
            },
            // Empréstimo devolvido - João pegou A Hora da Estrela
            new() 
            { 
                Id = 2, 
                LivroId = 3, 
                UsuarioId = 2, 
                DataEmprestimo = DateTime.Now.AddDays(-20), 
                DataDevolucaoPrevista = DateTime.Now.AddDays(-6), 
                DataDevolucaoReal = DateTime.Now.AddDays(-7), 
                Status = EmprestimoStatus.Devolvido 
            },
            // Empréstimo ativo - Ana pegou Capitães da Areia
            new() 
            { 
                Id = 3, 
                LivroId = 5, 
                UsuarioId = 3, 
                DataEmprestimo = DateTime.Now.AddDays(-3), 
                DataDevolucaoPrevista = DateTime.Now.AddDays(11), 
                DataDevolucaoReal = null, 
                Status = EmprestimoStatus.Ativo 
            },
            // Empréstimo atrasado - Carlos pegou O Alquimista há muito tempo
            new() 
            { 
                Id = 4, 
                LivroId = 7, 
                UsuarioId = 4, 
                DataEmprestimo = DateTime.Now.AddDays(-25), 
                DataDevolucaoPrevista = DateTime.Now.AddDays(-11), 
                DataDevolucaoReal = null, 
                Status = EmprestimoStatus.Atrasado 
            }
        };
        context.Emprestimos.AddRange(emprestimos);

        // === RESERVAS ===
        var reservas = new List<Reserva>
        {
            // Reserva ativa - João quer Dom Casmurro (que está emprestado)
            new() 
            { 
                Id = 1, 
                LivroId = 1, 
                UsuarioId = 2, 
                DataReserva = DateTime.Now.AddDays(-2), 
                Status = ReservaStatus.Ativa 
            },
            // Reserva ativa - Maria quer Gabriela, Cravo e Canela
            new() 
            { 
                Id = 2, 
                LivroId = 6, 
                UsuarioId = 1, 
                DataReserva = DateTime.Now.AddDays(-1), 
                Status = ReservaStatus.Ativa 
            },
            // Reserva atendida - Ana reservou O Alquimista e foi atendida
            new() 
            { 
                Id = 3, 
                LivroId = 7, 
                UsuarioId = 3, 
                DataReserva = DateTime.Now.AddDays(-10), 
                Status = ReservaStatus.Atendida 
            }
        };
        context.Reservas.AddRange(reservas);

        // Salva tudo no banco
        context.SaveChanges();
    }
}
