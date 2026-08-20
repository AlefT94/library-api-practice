using LibraryApi.Domain.Entities;
using LibraryApi.Domain.Interfaces;
using LibraryApi.Infrastructure.Persistence;

namespace LibraryApi.Infrastructure.Repositories;

public class EmprestimoRepository(LibraryDbContext context) : Repository<Emprestimo>(context), IEmprestimoRepository
{
}
