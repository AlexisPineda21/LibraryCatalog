using LibraryCatalog.Application.Contracts.Repositories;
using LibraryCatalog.Application.Queries.DTOs;

namespace LibraryCatalog.Application.Queries.GetBooksList;

public sealed class GetBooksListHandler
{
    private readonly ILibroRepository _libroRepository;

    public GetBooksListHandler(ILibroRepository libroRepository)
    {
        _libroRepository = libroRepository;
    }

    public async Task<IReadOnlyCollection<LibroDto>> HandleAsync(
        GetBooksListQuery query,
        CancellationToken cancellationToken = default)
    {
        var libros = await _libroRepository.ObtenerTodosAsync(cancellationToken);

        return libros
            .Select(libro => new LibroDto(
                libro.Id,
                libro.Titulo,
                libro.ISBN,
                libro.AnioPublicacion,
                libro.AutorId,
                libro.Autor.Nombre,
                libro.CategoriaId,
                libro.Categoria.Nombre))
            .ToList();
    }
}
