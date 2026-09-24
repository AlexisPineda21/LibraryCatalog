using LibraryCatalog.Domain.Entities;

namespace LibraryCatalog.Application.Contracts.Repositories;

public interface ILibroRepository
{
    Task<IReadOnlyCollection<Libro>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default);

    Task<Libro?> ObtenerPorIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Libro>> ObtenerPorCategoriaAsync(
        Guid categoriaId,
        CancellationToken cancellationToken = default);
}
