using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GradSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregarSiglasUniversidad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Siglas",
                table: "Universidades",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Universidades",
                keyColumn: "Id",
                keyValue: 1,
                column: "Siglas",
                value: "UANL");

            migrationBuilder.UpdateData(
                table: "Universidades",
                keyColumn: "Id",
                keyValue: 2,
                column: "Siglas",
                value: "UIN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Siglas",
                table: "Universidades");
        }
    }
}
