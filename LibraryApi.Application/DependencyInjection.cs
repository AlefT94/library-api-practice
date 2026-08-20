using LibraryApi.Application.Interfaces;
using LibraryApi.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAutorService, AutorService>();
        services.AddScoped<ICategoriaService, CategoriaService>();
        services.AddScoped<ILivroService, LivroService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IEmprestimoService, EmprestimoService>();
        services.AddScoped<IReservaService, ReservaService>();
        return services;
    }
}
