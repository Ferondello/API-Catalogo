using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopularCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Categorias(Nome, ImagemUrl) VALUES('Bebidas','bebidas.jpeg')");
            migrationBuilder.Sql("INSERT INTO Categorias(Nome, ImagemUrl) VALUES('Lanches','lanches.jpeg')");
            migrationBuilder.Sql("INSERT INTO Categorias(Nome, ImagemUrl) VALUES('Sobremesas','sobremesas.jpeg')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Categorias");
        }
    }
}
