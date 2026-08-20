using LibraryApi.Domain.Interfaces;
using LibraryApi.Infrastructure.Persistence;
using LibraryApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryApi.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<LibraryDbContext>(options =>
            options.UseInMemoryDatabase("LibraryDb"));

        services.AddScoped<IAutorRepository, AutorRepository>();
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        services.AddScoped<ILivroRepository, LivroRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IEmprestimoRepository, EmprestimoRepository>();
        services.AddScoped<IReservaRepository, ReservaRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
