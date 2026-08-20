using LibraryApi.Application.DTOs;
using LibraryApi.Application.Interfaces;
using LibraryApi.Domain.Entities;
using LibraryApi.Domain.Interfaces;

namespace LibraryApi.Application.Services;

public class LivroService(IUnitOfWork unitOfWork) : ILivroService
{
    public async Task<IEnumerable<LivroResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var livros = await unitOfWork.Livros.GetAllAsync(cancellationToken);
        return livros.Select(Map);
    }

    public async Task<LivroResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var livro = await unitOfWork.Livros.GetByIdAsync(id, cancellationToken);
        return livro is null ? null : Map(livro);
    }

    public async Task<LivroResponseDto> CreateAsync(CreateLivroDto dto, CancellationToken cancellationToken = default)
    {
        await ValidarLivroAsync(dto.ISBN, dto.AutorId, dto.CategoriaId, null, dto.QuantidadeTotal, dto.QuantidadeDisponivel, cancellationToken);

        var livro = new Livro
        {
            Titulo = dto.Titulo.Trim(),
            ISBN = dto.ISBN.Trim(),
            AnoPublicacao = dto.AnoPublicacao,
            QuantidadeTotal = dto.QuantidadeTotal,
            QuantidadeDisponivel = dto.QuantidadeDisponivel,
            AutorId = dto.AutorId,
            CategoriaId = dto.CategoriaId
        };

        await unitOfWork.Livros.AddAsync(livro, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(livro);
    }

    public async Task<bool> UpdateAsync(int id, UpdateLivroDto dto, CancellationToken cancellationToken = default)
    {
        var livro = await unitOfWork.Livros.GetByIdAsync(id, cancellationToken);
        if (livro is null)
        {
            return false;
        }

        await ValidarLivroAsync(dto.ISBN, dto.AutorId, dto.CategoriaId, id, dto.QuantidadeTotal, dto.QuantidadeDisponivel, cancellationToken);

        livro.Titulo = dto.Titulo.Trim();
        livro.ISBN = dto.ISBN.Trim();
        livro.AnoPublicacao = dto.AnoPublicacao;
        livro.QuantidadeTotal = dto.QuantidadeTotal;
        livro.QuantidadeDisponivel = dto.QuantidadeDisponivel;
        livro.AutorId = dto.AutorId;
        livro.CategoriaId = dto.CategoriaId;

        unitOfWork.Livros.Update(livro);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var livro = await unitOfWork.Livros.GetByIdAsync(id, cancellationToken);
        if (livro is null)
        {
            return false;
        }

        unitOfWork.Livros.Delete(livro);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    private async Task ValidarLivroAsync(
        string isbn,
        int autorId,
        int categoriaId,
        int? ignoreId,
        int quantidadeTotal,
        int quantidadeDisponivel,
        CancellationToken cancellationToken)
    {
        if (await unitOfWork.Livros.ExistsByIsbnAsync(isbn.Trim(), ignoreId, cancellationToken))
        {
            throw new InvalidOperationException("Já existe um livro com este ISBN.");
        }

        if (quantidadeDisponivel > quantidadeTotal)
        {
            throw new InvalidOperationException("A quantidade disponível não pode ser maior que a quantidade total.");
        }

        if (await unitOfWork.Autores.GetByIdAsync(autorId, cancellationToken) is null)
        {
            throw new InvalidOperationException("Autor informado não existe.");
        }

        if (await unitOfWork.Categorias.GetByIdAsync(categoriaId, cancellationToken) is null)
        {
            throw new InvalidOperationException("Categoria informada não existe.");
        }
    }

    private static LivroResponseDto Map(Livro livro) =>
        new(livro.Id, livro.Titulo, livro.ISBN, livro.AnoPublicacao, livro.QuantidadeTotal, livro.QuantidadeDisponivel, livro.AutorId, livro.CategoriaId);
}
