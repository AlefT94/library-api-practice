using LibraryApi.Domain.Entities;
using LibraryApi.Domain.Interfaces;
using LibraryApi.Infrastructure.Persistence;

namespace LibraryApi.Infrastructure.Repositories;

public class ReservaRepository(LibraryDbContext context) : Repository<Reserva>(context), IReservaRepository
{
}
