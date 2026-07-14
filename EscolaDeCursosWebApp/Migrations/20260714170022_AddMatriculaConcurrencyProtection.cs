using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscolaDeCursosWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddMatriculaConcurrencyProtection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "UX_TBMatricula_AlunoId_TurmaId",
                table: "TBMatricula",
                columns: new[] { "AlunoId", "TurmaId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_TBMatricula_AlunoId_TurmaId",
                table: "TBMatricula");
        }
    }
}