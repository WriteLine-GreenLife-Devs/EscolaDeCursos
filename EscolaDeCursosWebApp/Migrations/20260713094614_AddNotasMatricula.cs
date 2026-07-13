using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscolaDeCursosWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddNotasMatricula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Nota1",
                table: "TBMatricula",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Nota2",
                table: "TBMatricula",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Nota3",
                table: "TBMatricula",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NotaFinal",
                table: "TBMatricula",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Recuperacao",
                table: "TBMatricula",
                type: "decimal(5,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nota1",
                table: "TBMatricula");

            migrationBuilder.DropColumn(
                name: "Nota2",
                table: "TBMatricula");

            migrationBuilder.DropColumn(
                name: "Nota3",
                table: "TBMatricula");

            migrationBuilder.DropColumn(
                name: "NotaFinal",
                table: "TBMatricula");

            migrationBuilder.DropColumn(
                name: "Recuperacao",
                table: "TBMatricula");
        }
    }
}
