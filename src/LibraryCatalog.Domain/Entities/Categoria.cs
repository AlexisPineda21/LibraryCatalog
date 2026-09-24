using LibraryCatalog.Domain.Exceptions;

namespace LibraryCatalog.Domain.Entities;

public sealed class Categoria
{
    private const int LongitudMaximaNombre = 100;
    private readonly List<Libro> _libros = [];

    public Guid Id { get; private set; }
    public string Nombre { get; private set; } = null!;
    public IReadOnlyCollection<Libro> Libros => _libros;

    private Categoria()
    {
    }

    public Categoria(string nombre)
    {
        Nombre = ValidarNombre(nombre);
        Id = Guid.CreateVersion7();
    }

    private static string ValidarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ReglaDeNegocioException("El nombre de la categoría es obligatorio.");
        }

        string nombreNormalizado = nombre.Trim();

        if (nombreNormalizado.Length > LongitudMaximaNombre)
        {
            throw new ReglaDeNegocioException(
                $"El nombre de la categoría no puede superar los {LongitudMaximaNombre} caracteres.");
        }

        return nombreNormalizado;
    }
}
