using FluentAssertions;
using LibraryApi.Application.DTOs;
using LibraryApi.Application.Services;
using LibraryApi.Domain.Entities;
using LibraryApi.Domain.Interfaces;
using Moq;

namespace LibraryApi.Application.Tests.Services;

public class AutorServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IAutorRepository> _autorRepositoryMock;
    private readonly AutorService _service;

    public AutorServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _autorRepositoryMock = new Mock<IAutorRepository>();

        _unitOfWorkMock
            .SetupGet(x => x.Autores)
            .Returns(_autorRepositoryMock.Object);

        _service = new AutorService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task CreateAsync_DeveCriarAutorComCamposSemEspacosEPersistir()
    {
        // Arrange
        var dto = new CreateAutorDto
        {
            Nome = "  Machado de Assis  ",
            Biografia = "  Escritor brasileiro  ",
            Pais = "  Brasil  "
        };

        _autorRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Autor>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var resultado = await _service.CreateAsync(dto);

        // Assert
        resultado.Nome.Should().Be("Machado de Assis");
        resultado.Biografia.Should().Be("Escritor brasileiro");
        resultado.Pais.Should().Be("Brasil");

        _autorRepositoryMock.Verify(
            x => x.AddAsync(
                It.Is<Autor>(autor =>
                    autor.Nome == "Machado de Assis" &&
                    autor.Biografia == "Escritor brasileiro" &&
                    autor.Pais == "Brasil"),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarAutor_QuandoAutorExiste()
    {
        // Arrange
        var autor = new Autor
        {
            Id = 1,
            Nome = "Machado de Assis",
            Biografia = "Escritor brasileiro",
            Pais = "Brasil"
        };

        _autorRepositoryMock
            .Setup(x => x.GetByIdAsync(autor.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(autor);

        // Act
        var resultado = await _service.GetByIdAsync(autor.Id);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Id.Should().Be(1);
        resultado.Nome.Should().Be("Machado de Assis");
        resultado.Biografia.Should().Be("Escritor brasileiro");
        resultado.Pais.Should().Be("Brasil");

        _autorRepositoryMock.Verify(
            x => x.GetByIdAsync(autor.Id, It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarNull_QuandoAutorNaoExiste()
    {
        //Arrange
        _autorRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Autor?)null);

        //Act
        var resultado = await _service.GetByIdAsync(999);

        //Assert
        resultado.Should().BeNull();

        _autorRepositoryMock.Verify(
            x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarTodosOsAutoresMapeados()
    {
        // Arrange
        var autores = new List<Autor>
    {
        new Autor
        {
            Id = 1,
            Nome = "Machado de Assis",
            Biografia = "Escritor brasileiro",
            Pais = "Brasil"
        },
        new Autor
        {
            Id = 2,
            Nome = "Clarice Lispector",
            Biografia = "Escritora brasileira",
            Pais = "Brasil"
        },
        new Autor
        {
            Id = 3,
            Nome = "Jorge Amado",
            Biografia = "Escritor baiano",
            Pais = "Brasil"
        }
    };

        _autorRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(autores);

        // Act
        var resultado = await _service.GetAllAsync();

        // Assert
        resultado.Should().HaveCount(3);

        var lista = resultado.ToList();

        lista[0].Id.Should().Be(1);
        lista[0].Nome.Should().Be("Machado de Assis");

        lista[1].Id.Should().Be(2);
        lista[1].Nome.Should().Be("Clarice Lispector");

        lista[2].Id.Should().Be(3);
        lista[2].Nome.Should().Be("Jorge Amado");

        _autorRepositoryMock.Verify(
            x => x.GetAllAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_DeveRetornarFalse_QuandoAutorNaoExiste()
    {
        //Arrange
        var dto = new UpdateAutorDto
        {
            Nome = "Novo Nome",
            Biografia = "Nova Biografia",
            Pais = "Novo País"
        };

        _autorRepositoryMock
            .Setup(x=>x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Autor?)null);

        //Act
        var result = await _service.UpdateAsync(999, dto);

        //Assert
        result.Should().Be(false);
        _autorRepositoryMock.Verify(
            x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}