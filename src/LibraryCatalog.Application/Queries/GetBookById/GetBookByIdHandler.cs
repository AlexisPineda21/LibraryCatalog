using LibraryCatalog.Application.Contracts.Repositories;
using LibraryCatalog.Application.Queries.DTOs;

namespace LibraryCatalog.Application.Queries.GetBookById;

public sealed class GetBookByIdHandler
{
    private readonly ILibroRepository _libroRepository;

    public GetBookByIdHandler(ILibroRepository libroRepository)
    {
        _libroRepository = libroRepository;
    }

    public async Task<LibroDto?> HandleAsync(
        GetBookByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var libro = await _libroRepository.ObtenerPorIdAsync(
            query.Id,
            cancellationToken);

        if (libro is null)
        {
            return null;
        }

        return new LibroDto(
            libro.Id,
            libro.Titulo,
            libro.ISBN,
            libro.AnioPublicacion,
            libro.AutorId,
            libro.Autor.Nombre,
            libro.CategoriaId,
            libro.Categoria.Nombre);
    }
}