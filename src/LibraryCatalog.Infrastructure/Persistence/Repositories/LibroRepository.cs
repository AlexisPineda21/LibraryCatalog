using LibraryCatalog.Application.Contracts.Repositories;
using LibraryCatalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryCatalog.Infrastructure.Persistence.Repositories;

internal sealed class LibroRepository : ILibroRepository
{
    private readonly LibraryCatalogDbContext _context;

    public LibroRepository(LibraryCatalogDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Libro>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default)
    {
        return await ConsultaBase()
            .OrderBy(libro => libro.Titulo)
            .ToListAsync(cancellationToken);
    }

    public async Task<Libro?> ObtenerPorIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await ConsultaBase()
            .SingleOrDefaultAsync(libro => libro.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Libro>> ObtenerPorCategoriaAsync(
        Guid categoriaId,
        CancellationToken cancellationToken = default)
    {
        return await ConsultaBase()
            .Where(libro => libro.CategoriaId == categoriaId)
            .OrderBy(libro => libro.Titulo)
            .ToListAsync(cancellationToken);
    }

    private IQueryable<Libro> ConsultaBase()
    {
        return _context.Libros
            .AsNoTracking()
            .Include(libro => libro.Autor)
            .Include(libro => libro.Categoria);
    }
}
