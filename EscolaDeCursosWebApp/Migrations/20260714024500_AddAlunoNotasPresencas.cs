using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscolaDeCursosWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAlunoNotasPresencas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBAluno",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBAluno", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBAluno_TBUsuario",
                        column: x => x.Id,
                        principalTable: "TBUsuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql(
                """
                INSERT INTO [TBAluno] ([Id])
                SELECT [Id]
                FROM [TBUsuario]
                WHERE [tipoUsuario] = 0;
                """);

            migrationBuilder.CreateTable(
                name: "TBNotaAluno",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AlunoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatriculaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoNota = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    DataLancamento = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBNotaAluno", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBNotaAluno_TBAluno",
                        column: x => x.AlunoId,
                        principalTable: "TBAluno",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TBNotaAluno_TBMatricula",
                        column: x => x.MatriculaId,
                        principalTable: "TBMatricula",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TBPresencaAluno",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AlunoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatriculaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataAula = table.Column<DateTime>(type: "date", nullable: false),
                    Presente = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBPresencaAluno", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBPresencaAluno_TBAluno",
                        column: x => x.AlunoId,
                        principalTable: "TBAluno",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TBPresencaAluno_TBMatricula",
                        column: x => x.MatriculaId,
                        principalTable: "TBMatricula",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBNotaAluno_AlunoId",
                table: "TBNotaAluno",
                column: "AlunoId");

            migrationBuilder.CreateIndex(
                name: "UQ_TBNotaAluno_MatriculaId_TipoNota",
                table: "TBNotaAluno",
                columns: new[] { "MatriculaId", "TipoNota" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TBPresencaAluno_AlunoId",
                table: "TBPresencaAluno",
                column: "AlunoId");

            migrationBuilder.CreateIndex(
                name: "UQ_TBPresencaAluno_MatriculaId_DataAula",
                table: "TBPresencaAluno",
                columns: new[] { "MatriculaId", "DataAula" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBNotaAluno");

            migrationBuilder.DropTable(
                name: "TBPresencaAluno");

            migrationBuilder.DropTable(
                name: "TBAluno");
        }
    }
}