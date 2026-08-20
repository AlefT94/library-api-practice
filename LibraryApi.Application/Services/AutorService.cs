using LibraryApi.Application.DTOs;
using LibraryApi.Application.Interfaces;
using LibraryApi.Domain.Entities;
using LibraryApi.Domain.Interfaces;

namespace LibraryApi.Application.Services;

public class AutorService(IUnitOfWork unitOfWork) : IAutorService
{
    public async Task<IEnumerable<AutorResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var autores = await unitOfWork.Autores.GetAllAsync(cancellationToken);
        return autores.Select(Map);
    }

    public async Task<AutorResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var autor = await unitOfWork.Autores.GetByIdAsync(id, cancellationToken);
        return autor is null ? null : Map(autor);
    }

    public async Task<AutorResponseDto> CreateAsync(CreateAutorDto dto, CancellationToken cancellationToken = default)
    {
        var autor = new Autor
        {
            Nome = dto.Nome.Trim(),
            Biografia = dto.Biografia?.Trim(),
            Pais = dto.Pais?.Trim()
        };

        await unitOfWork.Autores.AddAsync(autor, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(autor);
    }

    public async Task<bool> UpdateAsync(int id, UpdateAutorDto dto, CancellationToken cancellationToken = default)
    {
        var autor = await unitOfWork.Autores.GetByIdAsync(id, cancellationToken);
        if (autor is null)
        {
            return false;
        }

        autor.Nome = dto.Nome.Trim();
        autor.Biografia = dto.Biografia?.Trim();
        autor.Pais = dto.Pais?.Trim();

        unitOfWork.Autores.Update(autor);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var autor = await unitOfWork.Autores.GetByIdAsync(id, cancellationToken);
        if (autor is null)
        {
            return false;
        }

        unitOfWork.Autores.Delete(autor);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static AutorResponseDto Map(Autor autor) => new(autor.Id, autor.Nome, autor.Biografia, autor.Pais);
}
