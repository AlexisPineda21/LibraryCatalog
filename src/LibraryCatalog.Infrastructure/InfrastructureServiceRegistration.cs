using LibraryCatalog.Application.Contracts.Repositories;
using LibraryCatalog.Infrastructure.Persistence;
using LibraryCatalog.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryCatalog.Infrastructure;

public static class InfrastructureServiceRegistration
{
    private const string NombreCadenaConexion = "LibraryCatalog";
    private const int MaximoReintentos = 3;
    private const int SegundosEsperaEntreReintentos = 5;
    private const int SegundosTiempoEsperaComando = 30;

    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        string cadenaConexion = ObtenerCadenaConexion(configuration);

        services.AddDbContext<LibraryCatalogDbContext>(options =>
            options.UseSqlServer(cadenaConexion, sqlServer =>
            {
                sqlServer.EnableRetryOnFailure(
                    MaximoReintentos,
                    TimeSpan.FromSeconds(SegundosEsperaEntreReintentos),
                    null);

                sqlServer.CommandTimeout(SegundosTiempoEsperaComando);
            }));

        services.AddScoped<ILibroRepository, LibroRepository>();

        return services;
    }

    private static string ObtenerCadenaConexion(IConfiguration configuration)
    {
        string? cadenaConexion = configuration.GetConnectionString(NombreCadenaConexion);

        if (string.IsNullOrWhiteSpace(cadenaConexion))
        {
            throw new InvalidOperationException(
                $"La cadena de conexión '{NombreCadenaConexion}' no está configurada. " +
                "Defínela en appsettings.json, en appsettings.Local.json o con dotnet user-secrets.");
        }

        return cadenaConexion;
    }
}
