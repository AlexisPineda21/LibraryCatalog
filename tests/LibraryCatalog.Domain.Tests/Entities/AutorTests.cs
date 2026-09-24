using LibraryCatalog.Domain.Entities;
using LibraryCatalog.Domain.Exceptions;

namespace LibraryCatalog.Domain.Tests.Entities;

[TestClass]
public sealed class AutorTests
{
    [TestMethod]
    public void Crear_ConNombreValido_DebeInicializarAutor()
    {
        Autor autor = new("  Gabriel García Márquez  ");

        Assert.AreNotEqual(Guid.Empty, autor.Id);
        Assert.AreEqual("Gabriel García Márquez", autor.Nombre);
        Assert.IsEmpty(autor.Libros);
    }

    [TestMethod]
    public void Crear_ConNombreVacio_DebeLanzarExcepcion()
    {
        Assert.ThrowsExactly<ReglaDeNegocioException>(() => new Autor("   "));
    }

    [TestMethod]
    public void Crear_ConNombreDemasiadoLargo_DebeLanzarExcepcion()
    {
        string nombre = new('a', 151);

        Assert.ThrowsExactly<ReglaDeNegocioException>(() => new Autor(nombre));
    }
}
