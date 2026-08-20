using LibraryApi.Application.DTOs;
using LibraryApi.Application.Interfaces;
using LibraryApi.Domain.Entities;
using LibraryApi.Domain.Enums;
using LibraryApi.Domain.Interfaces;

namespace LibraryApi.Application.Services;

public class EmprestimoService(IUnitOfWork unitOfWork) : IEmprestimoService
{
    public async Task<IEnumerable<EmprestimoResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var emprestimos = await unitOfWork.Emprestimos.GetAllAsync(cancellationToken);
        return emprestimos.Select(Map);
    }

    public async Task<EmprestimoResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var emprestimo = await unitOfWork.Emprestimos.GetByIdAsync(id, cancellationToken);
        return emprestimo is null ? null : Map(emprestimo);
    }

    public async Task<EmprestimoResponseDto> CreateAsync(CreateEmprestimoDto dto, CancellationToken cancellationToken = default)
    {
        var livro = await unitOfWork.Livros.GetByIdAsync(dto.LivroId, cancellationToken)
                    ?? throw new InvalidOperationException("Livro informado não existe.");

        _ = await unitOfWork.Usuarios.GetByIdAsync(dto.UsuarioId, cancellationToken)
            ?? throw new InvalidOperationException("Usuário informado não existe.");

        if (livro.QuantidadeDisponivel <= 0)
        {
            throw new InvalidOperationException("Livro indisponível para empréstimo.");
        }

        var dataEmprestimo = dto.DataEmprestimo ?? DateOnly.FromDateTime(DateTime.UtcNow.Date);
        if (dto.DataDevolucaoPrevista <= dataEmprestimo)
        {
            throw new InvalidOperationException("Data de devolução prevista deve ser maior que a data de empréstimo.");
        }

        livro.QuantidadeDisponivel -= 1;
        unitOfWork.Livros.Update(livro);

        var emprestimo = new Emprestimo
        {
            LivroId = dto.LivroId,
            UsuarioId = dto.UsuarioId,
            DataEmprestimo = dataEmprestimo,
            DataDevolucaoPrevista = dto.DataDevolucaoPrevista,
            Status = EmprestimoStatus.Ativo
        };

        await unitOfWork.Emprestimos.AddAsync(emprestimo, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(emprestimo);
    }

    public async Task<bool> UpdateAsync(int id, UpdateEmprestimoDto dto, CancellationToken cancellationToken = default)
    {
        var emprestimo = await unitOfWork.Emprestimos.GetByIdAsync(id, cancellationToken);
        if (emprestimo is null)
        {
            return false;
        }

        if (dto.DataDevolucaoPrevista <= dto.DataEmprestimo)
        {
            throw new InvalidOperationException("Data de devolução prevista deve ser maior que a data de empréstimo.");
        }

        var livro = await unitOfWork.Livros.GetByIdAsync(dto.LivroId, cancellationToken)
                    ?? throw new InvalidOperationException("Livro informado não existe.");

        _ = await unitOfWork.Usuarios.GetByIdAsync(dto.UsuarioId, cancellationToken)
            ?? throw new InvalidOperationException("Usuário informado não existe.");

        if (emprestimo.LivroId != dto.LivroId)
        {
            var livroAtual = await unitOfWork.Livros.GetByIdAsync(emprestimo.LivroId, cancellationToken)
                            ?? throw new InvalidOperationException("Livro atual do empréstimo não encontrado.");

            if (emprestimo.Status != EmprestimoStatus.Devolvido)
            {
                livroAtual.QuantidadeDisponivel += 1;
                if (livro.QuantidadeDisponivel <= 0)
                {
                    throw new InvalidOperationException("Novo livro indisponível para empréstimo.");
                }
                livro.QuantidadeDisponivel -= 1;
                unitOfWork.Livros.Update(livroAtual);
            }
        }

        await AtualizarQuantidadePorMudancaStatusAsync(emprestimo, dto.Status, livro, cancellationToken);

        emprestimo.LivroId = dto.LivroId;
        emprestimo.UsuarioId = dto.UsuarioId;
        emprestimo.DataEmprestimo = dto.DataEmprestimo;
        emprestimo.DataDevolucaoPrevista = dto.DataDevolucaoPrevista;
        emprestimo.DataDevolucaoReal = dto.DataDevolucaoReal;
        emprestimo.Status = dto.Status;

        unitOfWork.Emprestimos.Update(emprestimo);
        unitOfWork.Livros.Update(livro);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var emprestimo = await unitOfWork.Emprestimos.GetByIdAsync(id, cancellationToken);
        if (emprestimo is null)
        {
            return false;
        }

        if (emprestimo.Status != EmprestimoStatus.Devolvido)
        {
            var livro = await unitOfWork.Livros.GetByIdAsync(emprestimo.LivroId, cancellationToken);
            if (livro is not null)
            {
                livro.QuantidadeDisponivel += 1;
                unitOfWork.Livros.Update(livro);
            }
        }

        unitOfWork.Emprestimos.Delete(emprestimo);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<EmprestimoResponseDto?> RegistrarDevolucaoAsync(int id, DateOnly? dataDevolucaoReal, CancellationToken cancellationToken = default)
    {
        var emprestimo = await unitOfWork.Emprestimos.GetByIdAsync(id, cancellationToken);
        if (emprestimo is null)
        {
            return null;
        }

        if (emprestimo.Status == EmprestimoStatus.Devolvido)
        {
            throw new InvalidOperationException("Este empréstimo já foi devolvido.");
        }

        var livro = await unitOfWork.Livros.GetByIdAsync(emprestimo.LivroId, cancellationToken)
                    ?? throw new InvalidOperationException("Livro do empréstimo não encontrado.");

        emprestimo.Status = EmprestimoStatus.Devolvido;
        emprestimo.DataDevolucaoReal = dataDevolucaoReal ?? DateOnly.FromDateTime(DateTime.UtcNow.Date);
        livro.QuantidadeDisponivel += 1;

        unitOfWork.Emprestimos.Update(emprestimo);
        unitOfWork.Livros.Update(livro);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(emprestimo);
    }

    private static EmprestimoResponseDto Map(Emprestimo emprestimo) =>
        new(
            emprestimo.Id,
            emprestimo.LivroId,
            emprestimo.UsuarioId,
            emprestimo.DataEmprestimo,
            emprestimo.DataDevolucaoPrevista,
            emprestimo.DataDevolucaoReal,
            emprestimo.Status);

    private static async Task AtualizarQuantidadePorMudancaStatusAsync(
        Emprestimo emprestimo,
        EmprestimoStatus novoStatus,
        Livro livro,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        if (emprestimo.Status == novoStatus)
        {
            return;
        }

        if (emprestimo.Status != EmprestimoStatus.Devolvido && novoStatus == EmprestimoStatus.Devolvido)
        {
            livro.QuantidadeDisponivel += 1;
            return;
        }

        if (emprestimo.Status == EmprestimoStatus.Devolvido && novoStatus != EmprestimoStatus.Devolvido)
        {
            if (livro.QuantidadeDisponivel <= 0)
            {
                throw new InvalidOperationException("Livro indisponível para reativar empréstimo.");
            }

            livro.QuantidadeDisponivel -= 1;
        }
    }
}
