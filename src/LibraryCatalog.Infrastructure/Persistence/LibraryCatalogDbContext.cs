using LibraryCatalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryCatalog.Infrastructure.Persistence;

public sealed class LibraryCatalogDbContext : DbContext
{
    public LibraryCatalogDbContext(DbContextOptions<LibraryCatalogDbContext> options)
        : base(options)
    {
    }

    public DbSet<Libro> Libros => Set<Libro>();
    public DbSet<Autor> Autores => Set<Autor>();
    public DbSet<Categoria> Categorias => Set<Categoria>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryCatalogDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
