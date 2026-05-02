using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GradSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregarDedicatorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dedicatorias",
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
                    table.PrimaryKey("PK_Dedicatorias", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Dedicatorias",
                columns: new[] { "Id", "Activo", "Nombre", "Orden" },
                values: new object[,]
                {
                    { 1, true, "A mis padres", 1 },
                    { 2, true, "A mi madre", 2 },
                    { 3, true, "A mi esposa", 3 },
                    { 4, true, "A mi esposo", 4 },
                    { 5, true, "A mis hijos", 5 },
                    { 6, true, "A mi familia", 6 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dedicatorias");
        }
    }
}
