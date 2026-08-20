using LibraryApi.Domain.Entities;
using LibraryApi.Domain.Interfaces;
using LibraryApi.Infrastructure.Persistence;

namespace LibraryApi.Infrastructure.Repositories;

public class CategoriaRepository(LibraryDbContext context) : Repository<Categoria>(context), ICategoriaRepository
{
}
