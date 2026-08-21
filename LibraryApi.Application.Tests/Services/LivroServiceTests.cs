using FluentAssertions;
using LibraryApi.Application.DTOs;
using LibraryApi.Application.Services;
using LibraryApi.Domain.Entities;
using LibraryApi.Domain.Interfaces;
using Moq;

namespace LibraryApi.Application.Tests.Services;

public class LivroServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILivroRepository> _livroRepository;
    private readonly Mock<IAutorRepository> _autoresRepository;
    private readonly Mock<ICategoriaRepository> _categoriaRepository;
    private readonly LivroService _service;

    public LivroServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _livroRepository = new Mock<ILivroRepository>();
        _autoresRepository = new Mock<IAutorRepository>();
        _categoriaRepository = new Mock<ICategoriaRepository>();

        _unitOfWorkMock
            .SetupGet(x => x.Livros)
            .Returns(_livroRepository.Object);

        _unitOfWorkMock
            .SetupGet(x => x.Autores)
            .Returns(_autoresRepository.Object);

        _unitOfWorkMock
            .SetupGet(x => x.Categorias)
            .Returns(_categoriaRepository.Object);

        _service = new LivroService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task CreateAsync_DeveDispararExcecao_QuandoJaExisteIsbn()
    {
        //Arrange
        var dto = new CreateLivroDto
        {
            Titulo = "Livro Teste",
            ISBN = "1234567890",
            AnoPublicacao = 2023,
            QuantidadeTotal = 10,
            QuantidadeDisponivel = 10,
            AutorId = 1,
            CategoriaId = 1
        };

        _livroRepository.Setup(x => x.ExistsByIsbnAsync(dto.ISBN))
            .ReturnsAsync(true);

        //Act
        var acao = () => _service.CreateAsync(dto);

        //Assert
        await acao.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Já existe um livro com este ISBN.");
    }

    [Fact]
    public async Task CreateAsync_DeveDispararExcecao_QuandoQuantidadeTotalMaiorQueDisponivel()
    {
        //Arrange
        var dto = new CreateLivroDto
        {
            Titulo = "Livro Teste",
            ISBN = "1234567890",
            AnoPublicacao = 2023,
            QuantidadeTotal = 10,
            QuantidadeDisponivel = 12,
            AutorId = 1,
            CategoriaId = 1
        };

        //Act
        var acao = () => _service.CreateAsync(dto);

        //Assert
        await acao.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("A quantidade disponível não pode ser maior que a quantidade total.");
    }

    [Fact]
    public async Task CreateAsync_DeveDispararExcecao_QuandoAutorInformadoNaoExiste()
    {
        //Arrange
        var dto = new CreateLivroDto
        {
            Titulo = "Livro Teste",
            ISBN = "1234567890",
            AnoPublicacao = 2023,
            QuantidadeTotal = 10,
            QuantidadeDisponivel = 10,
            AutorId = 1,
            CategoriaId = 1
        };

        _autoresRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Autor?)null);

        //Act
        var acao = () => _service.CreateAsync(dto);

        //Assert
        await acao.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Autor informado não existe.");

        _autoresRepository.Verify(x => x.GetByIdAsync(dto.AutorId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_DeveCriarLivro_QuandoInformadoDadosCorretos()
    {
        //Arrange
        var dto = new CreateLivroDto
        {
            Titulo = "Livro Teste",
            ISBN = "1234567890",
            AnoPublicacao = 2023,
            QuantidadeTotal = 20,
            QuantidadeDisponivel = 10,
            AutorId = 1,
            CategoriaId = 1
        };

        _livroRepository
        .Setup(x => x.ExistsByIsbnAsync(
            dto.ISBN,
            null,
            It.IsAny<CancellationToken>()))
        .ReturnsAsync(false);

        _autoresRepository.Setup(x => x.GetByIdAsync(dto.AutorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Autor { Id = dto.AutorId, Nome = "Autor Teste" });

        _categoriaRepository.Setup(x => x.GetByIdAsync(dto.CategoriaId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Categoria { Id = dto.CategoriaId, Nome = "Categoria Teste" });

        //Act
        var result = await _service.CreateAsync(dto);

        //Assert
        result.Titulo.Should().Be(dto.Titulo);
        result.ISBN.Should().Be(dto.ISBN);
        result.AnoPublicacao.Should().Be(dto.AnoPublicacao);
        result.QuantidadeTotal.Should().Be(dto.QuantidadeTotal);
        result.QuantidadeDisponivel.Should().Be(dto.QuantidadeDisponivel);
        result.AutorId.Should().Be(dto.AutorId);
        result.CategoriaId.Should().Be(dto.CategoriaId);

        _livroRepository.Verify(
            x => x.AddAsync(
                It.Is<Livro>(livro =>
                    livro.Titulo == dto.Titulo &&
                    livro.ISBN == dto.ISBN &&
                    livro.AnoPublicacao == dto.AnoPublicacao &&
                    livro.QuantidadeTotal == dto.QuantidadeTotal &&
                    livro.QuantidadeDisponivel == dto.QuantidadeDisponivel &&
                    livro.AutorId == dto.AutorId &&
                    livro.CategoriaId == dto.CategoriaId),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

    }
}
