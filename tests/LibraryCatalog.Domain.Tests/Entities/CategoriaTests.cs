using LibraryCatalog.Domain.Entities;
using LibraryCatalog.Domain.Exceptions;

namespace LibraryCatalog.Domain.Tests.Entities;

[TestClass]
public sealed class CategoriaTests
{
    [TestMethod]
    public void Crear_ConNombreValido_DebeInicializarCategoria()
    {
        Categoria categoria = new("  Literatura latinoamericana  ");

        Assert.AreNotEqual(Guid.Empty, categoria.Id);
        Assert.AreEqual("Literatura latinoamericana", categoria.Nombre);
        Assert.IsEmpty(categoria.Libros);
    }

    [TestMethod]
    public void Crear_ConNombreVacio_DebeLanzarExcepcion()
    {
        Assert.ThrowsExactly<ReglaDeNegocioException>(() => new Categoria(string.Empty));
    }

    [TestMethod]
    public void Crear_ConNombreDemasiadoLargo_DebeLanzarExcepcion()
    {
        string nombre = new('a', 101);

        Assert.ThrowsExactly<ReglaDeNegocioException>(() => new Categoria(nombre));
    }
}
