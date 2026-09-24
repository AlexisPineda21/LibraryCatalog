using LibraryCatalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryCatalog.Infrastructure.Persistence.Configurations;

internal sealed class AutorConfiguration : IEntityTypeConfiguration<Autor>
{
    public void Configure(EntityTypeBuilder<Autor> builder)
    {
        builder.ToTable("Autores");

        builder.HasKey(autor => autor.Id);

        builder.Property(autor => autor.Nombre)
            .HasMaxLength(150)
            .IsRequired();

        builder.Navigation(autor => autor.Libros)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
