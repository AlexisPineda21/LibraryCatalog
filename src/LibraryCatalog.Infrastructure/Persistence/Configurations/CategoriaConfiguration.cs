using LibraryCatalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryCatalog.Infrastructure.Persistence.Configurations;

internal sealed class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("Categorias");

        builder.HasKey(categoria => categoria.Id);

        builder.Property(categoria => categoria.Nombre)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(categoria => categoria.Nombre)
            .IsUnique();

        builder.Navigation(categoria => categoria.Libros)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
