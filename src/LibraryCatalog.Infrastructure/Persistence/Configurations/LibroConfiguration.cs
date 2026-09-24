using LibraryCatalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryCatalog.Infrastructure.Persistence.Configurations;

internal sealed class LibroConfiguration : IEntityTypeConfiguration<Libro>
{
    public void Configure(EntityTypeBuilder<Libro> builder)
    {
        builder.ToTable("Libros");

        builder.HasKey(libro => libro.Id);

        builder.Property(libro => libro.Titulo)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(libro => libro.ISBN)
            .HasMaxLength(13)
            .IsRequired();

        builder.HasIndex(libro => libro.ISBN)
            .IsUnique();

        builder.Property(libro => libro.AnioPublicacion)
            .IsRequired();

        builder.HasOne(libro => libro.Autor)
            .WithMany(autor => autor.Libros)
            .HasForeignKey(libro => libro.AutorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(libro => libro.Categoria)
            .WithMany(categoria => categoria.Libros)
            .HasForeignKey(libro => libro.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
