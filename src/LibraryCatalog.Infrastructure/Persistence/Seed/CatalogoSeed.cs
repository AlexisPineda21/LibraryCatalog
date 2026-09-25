using LibraryCatalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryCatalog.Infrastructure.Persistence.Seed;


internal static class CatalogoSeed
{
    private static readonly Guid GarciaMarquez = Guid.Parse("a0000000-0000-4000-8000-000000000001");
    private static readonly Guid Allende = Guid.Parse("a0000000-0000-4000-8000-000000000002");
    private static readonly Guid Borges = Guid.Parse("a0000000-0000-4000-8000-000000000003");
    private static readonly Guid Verne = Guid.Parse("a0000000-0000-4000-8000-000000000004");
    private static readonly Guid Martin = Guid.Parse("a0000000-0000-4000-8000-000000000005");
    private static readonly Guid Harari = Guid.Parse("a0000000-0000-4000-8000-000000000006");

    private static readonly Guid Novela = Guid.Parse("c0000000-0000-4000-8000-000000000001");
    private static readonly Guid Cuento = Guid.Parse("c0000000-0000-4000-8000-000000000002");
    private static readonly Guid CienciaFiccion = Guid.Parse("c0000000-0000-4000-8000-000000000003");
    private static readonly Guid IngenieriaDeSoftware = Guid.Parse("c0000000-0000-4000-8000-000000000004");
    private static readonly Guid Historia = Guid.Parse("c0000000-0000-4000-8000-000000000005");

    /// <summary>
    /// Registra autores, categorías y libros de prueba en el modelo.
    /// </summary>
    public static ModelBuilder SembrarCatalogo(this ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        SembrarAutores(modelBuilder);
        SembrarCategorias(modelBuilder);
        SembrarLibros(modelBuilder);

        return modelBuilder;
    }

    private static void SembrarAutores(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Autor>().HasData(
            new { Id = GarciaMarquez, Nombre = "Gabriel García Márquez" },
            new { Id = Allende, Nombre = "Isabel Allende" },
            new { Id = Borges, Nombre = "Jorge Luis Borges" },
            new { Id = Verne, Nombre = "Julio Verne" },
            new { Id = Martin, Nombre = "Robert C. Martin" },
            new { Id = Harari, Nombre = "Yuval Noah Harari" });
    }

    private static void SembrarCategorias(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>().HasData(
            new { Id = Novela, Nombre = "Novela" },
            new { Id = Cuento, Nombre = "Cuento" },
            new { Id = CienciaFiccion, Nombre = "Ciencia ficción" },
            new { Id = IngenieriaDeSoftware, Nombre = "Ingeniería de software" },
            new { Id = Historia, Nombre = "Historia" });
    }

    private static void SembrarLibros(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Libro>().HasData(
            CrearLibro(1, "Cien años de soledad", "9788439700012", 1967, GarciaMarquez, Novela),
            CrearLibro(2, "El amor en los tiempos del cólera", "9788439700029", 1985, GarciaMarquez, Novela),
            CrearLibro(3, "La casa de los espíritus", "9788498385014", 1982, Allende, Novela),
            CrearLibro(4, "Ficciones", "9789500401012", 1944, Borges, Cuento),
            CrearLibro(5, "El Aleph", "9789500401029", 1949, Borges, Cuento),
            CrearLibro(6, "Veinte mil leguas de viaje submarino", "9788413229713", 1870, Verne, CienciaFiccion),
            CrearLibro(7, "De la Tierra a la Luna", "9788413229720", 1865, Verne, CienciaFiccion),
            CrearLibro(8, "Clean Code", "9780132350884", 2008, Martin, IngenieriaDeSoftware),
            CrearLibro(9, "Clean Architecture", "9780134494166", 2017, Martin, IngenieriaDeSoftware),
            CrearLibro(10, "Sapiens: de animales a dioses", "9788466333412", 2011, Harari, Historia));
    }

    
    private static object CrearLibro(
        int consecutivo,
        string titulo,
        string isbn,
        int anioPublicacion,
        Guid autorId,
        Guid categoriaId)
    {
        return new
        {
            Id = Guid.Parse($"b0000000-0000-4000-8000-{consecutivo:D12}"),
            Titulo = titulo,
            ISBN = isbn,
            AnioPublicacion = anioPublicacion,
            AutorId = autorId,
            CategoriaId = categoriaId,
        };
    }
}
