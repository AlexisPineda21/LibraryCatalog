namespace LibraryCatalog.Application.Queries.DTOs;

public sealed record LibroDto(
    Guid Id,
    string Titulo,
    string ISBN,
    int AnioPublicacion,
    Guid AutorId,
    string Autor,
    Guid CategoriaId,
    string Categoria);