using LibraryCatalog.Domain.Exceptions;

namespace LibraryCatalog.Domain.Entities;

public sealed class Libro
{
    private const int LongitudMaximaTitulo = 256;

    public Guid Id { get; private set; }
    public string Titulo { get; private set; } = null!;
    public string ISBN { get; private set; } = null!;
    public int AnioPublicacion { get; private set; }
    public Guid AutorId { get; private set; }
    public Autor Autor { get; private set; } = null!;
    public Guid CategoriaId { get; private set; }
    public Categoria Categoria { get; private set; } = null!;

    private Libro()
    {
    }

    public Libro(
        string titulo,
        string isbn,
        int anioPublicacion,
        Guid autorId,
        Guid categoriaId)
    {
        Titulo = ValidarTitulo(titulo);
        ISBN = ValidarYNormalizarIsbn(isbn);
        AnioPublicacion = ValidarAnioPublicacion(anioPublicacion);
        AutorId = ValidarIdentificador(autorId, "autor");
        CategoriaId = ValidarIdentificador(categoriaId, "categoría");
        Id = Guid.CreateVersion7();
    }

    private static string ValidarTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
        {
            throw new ReglaDeNegocioException("El título del libro es obligatorio.");
        }

        string tituloNormalizado = titulo.Trim();

        if (tituloNormalizado.Length > LongitudMaximaTitulo)
        {
            throw new ReglaDeNegocioException(
                $"El título del libro no puede superar los {LongitudMaximaTitulo} caracteres.");
        }

        return tituloNormalizado;
    }

    private static string ValidarYNormalizarIsbn(string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
        {
            throw new ReglaDeNegocioException("El ISBN del libro es obligatorio.");
        }

        string isbnNormalizado = isbn
            .Replace("-", string.Empty, StringComparison.Ordinal)
            .Replace(" ", string.Empty, StringComparison.Ordinal)
            .ToUpperInvariant();

        bool esIsbn13 = isbnNormalizado.Length == 13 && isbnNormalizado.All(char.IsDigit);
        bool esIsbn10 = isbnNormalizado.Length == 10
            && isbnNormalizado[..9].All(char.IsDigit)
            && (char.IsDigit(isbnNormalizado[9]) || isbnNormalizado[9] == 'X');

        if (!esIsbn10 && !esIsbn13)
        {
            throw new ReglaDeNegocioException("El ISBN debe tener un formato ISBN-10 o ISBN-13 válido.");
        }

        return isbnNormalizado;
    }

    private static int ValidarAnioPublicacion(int anioPublicacion)
    {
        if (anioPublicacion <= 0 || anioPublicacion > DateTime.UtcNow.Year)
        {
            throw new ReglaDeNegocioException(
                $"El año de publicación debe estar entre 1 y {DateTime.UtcNow.Year}.");
        }

        return anioPublicacion;
    }

    private static Guid ValidarIdentificador(Guid identificador, string entidadRelacionada)
    {
        if (identificador == Guid.Empty)
        {
            throw new ReglaDeNegocioException($"El identificador del {entidadRelacionada} es obligatorio.");
        }

        return identificador;
    }
}
