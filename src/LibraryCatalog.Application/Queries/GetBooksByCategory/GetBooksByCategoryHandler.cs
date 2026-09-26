using LibraryCatalog.Application.Contracts.Repositories;
using LibraryCatalog.Application.Queries.DTOs;

namespace LibraryCatalog.Application.Queries.GetBooksByCategory;

public sealed class GetBooksByCategoryHandler
{
    private readonly ILibroRepository _libroRepository;

    public GetBooksByCategoryHandler(ILibroRepository libroRepository)
    {
        _libroRepository = libroRepository;
    }

    public async Task<IReadOnlyCollection<LibroDto>> HandleAsync(
        GetBooksByCategoryQuery query,
        CancellationToken cancellationToken = default)
    {
        var libros = await _libroRepository.ObtenerPorCategoriaAsync(
            query.CategoriaId,
            cancellationToken);

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