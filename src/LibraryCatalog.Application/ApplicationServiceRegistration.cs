using LibraryCatalog.Application.Queries.GetBookById;
using LibraryCatalog.Application.Queries.GetBooksByCategory;
using LibraryCatalog.Application.Queries.GetBooksList;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryCatalog.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<GetBooksListHandler>();
        services.AddScoped<GetBookByIdHandler>();
        services.AddScoped<GetBooksByCategoryHandler>();

        return services;
    }
}
