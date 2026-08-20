using LibraryApi.Application.DTOs;
using LibraryApi.Application.Interfaces;
using LibraryApi.Domain.Entities;
using LibraryApi.Domain.Interfaces;

namespace LibraryApi.Application.Services;

public class CategoriaService(IUnitOfWork unitOfWork) : ICategoriaService
{
    public async Task<IEnumerable<CategoriaResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categorias = await unitOfWork.Categorias.GetAllAsync(cancellationToken);
        return categorias.Select(Map);
    }

    public async Task<CategoriaResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var categoria = await unitOfWork.Categorias.GetByIdAsync(id, cancellationToken);
        return categoria is null ? null : Map(categoria);
    }

    public async Task<CategoriaResponseDto> CreateAsync(CreateCategoriaDto dto, CancellationToken cancellationToken = default)
    {
        var categoria = new Categoria
        {
            Nome = dto.Nome.Trim(),
            Descricao = dto.Descricao?.Trim()
        };

        await unitOfWork.Categorias.AddAsync(categoria, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(categoria);
    }

    public async Task<bool> UpdateAsync(int id, UpdateCategoriaDto dto, CancellationToken cancellationToken = default)
    {
        var categoria = await unitOfWork.Categorias.GetByIdAsync(id, cancellationToken);
        if (categoria is null)
        {
            return false;
        }

        categoria.Nome = dto.Nome.Trim();
        categoria.Descricao = dto.Descricao?.Trim();

        unitOfWork.Categorias.Update(categoria);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var categoria = await unitOfWork.Categorias.GetByIdAsync(id, cancellationToken);
        if (categoria is null)
        {
            return false;
        }

        unitOfWork.Categorias.Delete(categoria);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static CategoriaResponseDto Map(Categoria categoria) => new(categoria.Id, categoria.Nome, categoria.Descricao);
}
