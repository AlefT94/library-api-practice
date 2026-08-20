using LibraryApi.Domain.Entities;
using LibraryApi.Domain.Interfaces;
using LibraryApi.Infrastructure.Persistence;

namespace LibraryApi.Infrastructure.Repositories;

public class AutorRepository(LibraryDbContext context) : Repository<Autor>(context), IAutorRepository
{
}
