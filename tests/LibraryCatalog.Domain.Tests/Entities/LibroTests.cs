using LibraryCatalog.Domain.Entities;
using LibraryCatalog.Domain.Exceptions;

namespace LibraryCatalog.Domain.Tests.Entities;

[TestClass]
public sealed class LibroTests
{
    [TestMethod]
    public void Crear_ConDatosValidos_DebeInicializarLibro()
    {
        Guid autorId = Guid.CreateVersion7();
        Guid categoriaId = Guid.CreateVersion7();

        Libro libro = new(
            "  Cien años de soledad  ",
            "978-0-306-40615-7",
            1967,
            autorId,
            categoriaId);

        Assert.AreNotEqual(Guid.Empty, libro.Id);
        Assert.AreEqual("Cien años de soledad", libro.Titulo);
        Assert.AreEqual("9780306406157", libro.ISBN);
        Assert.AreEqual(1967, libro.AnioPublicacion);
        Assert.AreEqual(autorId, libro.AutorId);
        Assert.AreEqual(categoriaId, libro.CategoriaId);
    }

    [TestMethod]
    public void Crear_ConIsbn10TerminadoEnX_DebeNormalizarIsbn()
    {
        Libro libro = CrearLibroValido("0-8044-2957-X");

        Assert.AreEqual("080442957X", libro.ISBN);
    }

    [TestMethod]
    public void Crear_ConTituloVacio_DebeLanzarExcepcion()
    {
        Assert.ThrowsExactly<ReglaDeNegocioException>(() => new Libro(
            " ",
            "9780306406157",
            2000,
            Guid.CreateVersion7(),
            Guid.CreateVersion7()));
    }

    [TestMethod]
    public void Crear_ConTituloDemasiadoLargo_DebeLanzarExcepcion()
    {
        Assert.ThrowsExactly<ReglaDeNegocioException>(() => new Libro(
            new string('a', 257),
            "9780306406157",
            2000,
            Guid.CreateVersion7(),
            Guid.CreateVersion7()));
    }

    [TestMethod]
    public void Crear_ConIsbnVacio_DebeLanzarExcepcion()
    {
        Assert.ThrowsExactly<ReglaDeNegocioException>(() => CrearLibroValido(string.Empty));
    }

    [TestMethod]
    public void Crear_ConFormatoIsbnInvalido_DebeLanzarExcepcion()
    {
        Assert.ThrowsExactly<ReglaDeNegocioException>(() => CrearLibroValido("ISBN-invalido"));
    }

    [TestMethod]
    public void Crear_ConAnioCero_DebeLanzarExcepcion()
    {
        Assert.ThrowsExactly<ReglaDeNegocioException>(() => new Libro(
            "Libro válido",
            "9780306406157",
            0,
            Guid.CreateVersion7(),
            Guid.CreateVersion7()));
    }

    [TestMethod]
    public void Crear_ConAnioFuturo_DebeLanzarExcepcion()
    {
        Assert.ThrowsExactly<ReglaDeNegocioException>(() => new Libro(
            "Libro válido",
            "9780306406157",
            DateTime.UtcNow.Year + 1,
            Guid.CreateVersion7(),
            Guid.CreateVersion7()));
    }

    [TestMethod]
    public void Crear_SinAutor_DebeLanzarExcepcion()
    {
        Assert.ThrowsExactly<ReglaDeNegocioException>(() => new Libro(
            "Libro válido",
            "9780306406157",
            2000,
            Guid.Empty,
            Guid.CreateVersion7()));
    }

    [TestMethod]
    public void Crear_SinCategoria_DebeLanzarExcepcion()
    {
        Assert.ThrowsExactly<ReglaDeNegocioException>(() => new Libro(
            "Libro válido",
            "9780306406157",
            2000,
            Guid.CreateVersion7(),
            Guid.Empty));
    }

    private static Libro CrearLibroValido(string isbn)
    {
        return new Libro(
            "Libro válido",
            isbn,
            2000,
            Guid.CreateVersion7(),
            Guid.CreateVersion7());
    }
}
