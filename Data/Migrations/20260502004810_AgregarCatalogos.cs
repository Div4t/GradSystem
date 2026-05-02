using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GradSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCatalogos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tallas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    Orden = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tallas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Universidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    Orden = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Universidades", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Tallas",
                columns: new[] { "Id", "Activo", "Nombre", "Orden" },
                values: new object[,]
                {
                    { 1, true, "XS", 1 },
                    { 2, true, "S", 2 },
                    { 3, true, "M", 3 },
                    { 4, true, "L", 4 },
                    { 5, true, "XL", 5 },
                    { 6, true, "XXL", 6 }
                });

            migrationBuilder.InsertData(
                table: "Universidades",
                columns: new[] { "Id", "Activo", "Nombre", "Orden" },
                values: new object[,]
                {
                    { 1, true, "UANL Universidad Autonoma de Nuevo Leon", 1 },
                    { 2, true, "UIN Universidad Interamericana del Norte", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tallas");

            migrationBuilder.DropTable(
                name: "Universidades");
        }
    }
}
