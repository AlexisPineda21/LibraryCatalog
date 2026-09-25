using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryCatalog.Infrastructure.Persistence.Migrations
{

    public partial class InitialCreate : Migration
    {
    
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Autores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Libros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    AnioPublicacion = table.Column<int>(type: "int", nullable: false),
                    AutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoriaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Libros_Autores_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Autores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Libros_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Autores",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { new Guid("a0000000-0000-4000-8000-000000000001"), "Gabriel García Márquez" },
                    { new Guid("a0000000-0000-4000-8000-000000000002"), "Isabel Allende" },
                    { new Guid("a0000000-0000-4000-8000-000000000003"), "Jorge Luis Borges" },
                    { new Guid("a0000000-0000-4000-8000-000000000004"), "Julio Verne" },
                    { new Guid("a0000000-0000-4000-8000-000000000005"), "Robert C. Martin" },
                    { new Guid("a0000000-0000-4000-8000-000000000006"), "Yuval Noah Harari" }
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { new Guid("c0000000-0000-4000-8000-000000000001"), "Novela" },
                    { new Guid("c0000000-0000-4000-8000-000000000002"), "Cuento" },
                    { new Guid("c0000000-0000-4000-8000-000000000003"), "Ciencia ficción" },
                    { new Guid("c0000000-0000-4000-8000-000000000004"), "Ingeniería de software" },
                    { new Guid("c0000000-0000-4000-8000-000000000005"), "Historia" }
                });

            migrationBuilder.InsertData(
                table: "Libros",
                columns: new[] { "Id", "AnioPublicacion", "AutorId", "CategoriaId", "ISBN", "Titulo" },
                values: new object[,]
                {
                    { new Guid("b0000000-0000-4000-8000-000000000001"), 1967, new Guid("a0000000-0000-4000-8000-000000000001"), new Guid("c0000000-0000-4000-8000-000000000001"), "9788439700012", "Cien años de soledad" },
                    { new Guid("b0000000-0000-4000-8000-000000000002"), 1985, new Guid("a0000000-0000-4000-8000-000000000001"), new Guid("c0000000-0000-4000-8000-000000000001"), "9788439700029", "El amor en los tiempos del cólera" },
                    { new Guid("b0000000-0000-4000-8000-000000000003"), 1982, new Guid("a0000000-0000-4000-8000-000000000002"), new Guid("c0000000-0000-4000-8000-000000000001"), "9788498385014", "La casa de los espíritus" },
                    { new Guid("b0000000-0000-4000-8000-000000000004"), 1944, new Guid("a0000000-0000-4000-8000-000000000003"), new Guid("c0000000-0000-4000-8000-000000000002"), "9789500401012", "Ficciones" },
                    { new Guid("b0000000-0000-4000-8000-000000000005"), 1949, new Guid("a0000000-0000-4000-8000-000000000003"), new Guid("c0000000-0000-4000-8000-000000000002"), "9789500401029", "El Aleph" },
                    { new Guid("b0000000-0000-4000-8000-000000000006"), 1870, new Guid("a0000000-0000-4000-8000-000000000004"), new Guid("c0000000-0000-4000-8000-000000000003"), "9788413229713", "Veinte mil leguas de viaje submarino" },
                    { new Guid("b0000000-0000-4000-8000-000000000007"), 1865, new Guid("a0000000-0000-4000-8000-000000000004"), new Guid("c0000000-0000-4000-8000-000000000003"), "9788413229720", "De la Tierra a la Luna" },
                    { new Guid("b0000000-0000-4000-8000-000000000008"), 2008, new Guid("a0000000-0000-4000-8000-000000000005"), new Guid("c0000000-0000-4000-8000-000000000004"), "9780132350884", "Clean Code" },
                    { new Guid("b0000000-0000-4000-8000-000000000009"), 2017, new Guid("a0000000-0000-4000-8000-000000000005"), new Guid("c0000000-0000-4000-8000-000000000004"), "9780134494166", "Clean Architecture" },
                    { new Guid("b0000000-0000-4000-8000-000000000010"), 2011, new Guid("a0000000-0000-4000-8000-000000000006"), new Guid("c0000000-0000-4000-8000-000000000005"), "9788466333412", "Sapiens: de animales a dioses" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Nombre",
                table: "Categorias",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Libros_AutorId",
                table: "Libros",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_CategoriaId",
                table: "Libros",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_ISBN",
                table: "Libros",
                column: "ISBN",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Libros");

            migrationBuilder.DropTable(
                name: "Autores");

            migrationBuilder.DropTable(
                name: "Categorias");
        }
    }
}
