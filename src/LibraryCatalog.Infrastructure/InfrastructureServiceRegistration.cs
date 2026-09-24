using LibraryCatalog.Application.Contracts.Repositories;
using LibraryCatalog.Infrastructure.Persistence;
using LibraryCatalog.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryCatalog.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddDbContext<LibraryCatalogDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("LibraryCatalog")));

        services.AddScoped<ILibroRepository, LibroRepository>();

        return services;
    }
}
