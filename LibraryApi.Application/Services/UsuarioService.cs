using LibraryApi.Application.DTOs;
using LibraryApi.Application.Interfaces;
using LibraryApi.Domain.Entities;
using LibraryApi.Domain.Interfaces;

namespace LibraryApi.Application.Services;

public class UsuarioService(IUnitOfWork unitOfWork) : IUsuarioService
{
    public async Task<IEnumerable<UsuarioResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var usuarios = await unitOfWork.Usuarios.GetAllAsync(cancellationToken);
        return usuarios.Select(Map);
    }

    public async Task<UsuarioResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var usuario = await unitOfWork.Usuarios.GetByIdAsync(id, cancellationToken);
        return usuario is null ? null : Map(usuario);
    }

    public async Task<UsuarioResponseDto> CreateAsync(CreateUsuarioDto dto, CancellationToken cancellationToken = default)
    {
        await ValidarUsuarioAsync(dto.Email, null, cancellationToken);

        var dataCadastro = dto.DataCadastro ?? DateOnly.FromDateTime(DateTime.UtcNow.Date);
        if (dataCadastro > DateOnly.FromDateTime(DateTime.UtcNow.Date))
        {
            throw new InvalidOperationException("Data de cadastro não pode estar no futuro.");
        }

        var usuario = new Usuario
        {
            Nome = dto.Nome.Trim(),
            Email = dto.Email.Trim(),
            Telefone = dto.Telefone?.Trim(),
            DataCadastro = dataCadastro
        };

        await unitOfWork.Usuarios.AddAsync(usuario, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Map(usuario);
    }

    public async Task<bool> UpdateAsync(int id, UpdateUsuarioDto dto, CancellationToken cancellationToken = default)
    {
        var usuario = await unitOfWork.Usuarios.GetByIdAsync(id, cancellationToken);
        if (usuario is null)
        {
            return false;
        }

        await ValidarUsuarioAsync(dto.Email, id, cancellationToken);

        if (dto.DataCadastro > DateOnly.FromDateTime(DateTime.UtcNow.Date))
        {
            throw new InvalidOperationException("Data de cadastro não pode estar no futuro.");
        }

        usuario.Nome = dto.Nome.Trim();
        usuario.Email = dto.Email.Trim();
        usuario.Telefone = dto.Telefone?.Trim();
        usuario.DataCadastro = dto.DataCadastro;

        unitOfWork.Usuarios.Update(usuario);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var usuario = await unitOfWork.Usuarios.GetByIdAsync(id, cancellationToken);
        if (usuario is null)
        {
            return false;
        }

        unitOfWork.Usuarios.Delete(usuario);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    private async Task ValidarUsuarioAsync(string email, int? ignoreId, CancellationToken cancellationToken)
    {
        if (await unitOfWork.Usuarios.ExistsByEmailAsync(email.Trim(), ignoreId, cancellationToken))
        {
            throw new InvalidOperationException("Já existe um usuário com este email.");
        }
    }

    private static UsuarioResponseDto Map(Usuario usuario) =>
        new(usuario.Id, usuario.Nome, usuario.Email, usuario.Telefone, usuario.DataCadastro);
}
