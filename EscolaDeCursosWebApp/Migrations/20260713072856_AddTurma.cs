using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscolaDeCursosWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddTurma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBTurma",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    dataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dataFim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    vagasMaximas = table.Column<int>(type: "int", nullable: false),
                    HorarioTurno = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    cursoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    instrutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBTurma", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBTurma_Nome",
                table: "TBTurma",
                column: "nome");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBTurma");
        }
    }
}
