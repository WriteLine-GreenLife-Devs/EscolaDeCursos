using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscolaDeCursosWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddModulosCursoEProgresso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBModuloCurso",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CursoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DuracaoMinutos = table.Column<int>(type: "int", nullable: false),
                    Ordem = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBModuloCurso", x => x.Id);
                    table.CheckConstraint("CK_TBModuloCurso_DuracaoMinutos", "[DuracaoMinutos] > 0");
                    table.CheckConstraint("CK_TBModuloCurso_Ordem", "[Ordem] > 0");
                    table.ForeignKey(
                        name: "FK_TBModuloCurso_TBCurso",
                        column: x => x.CursoId,
                        principalTable: "TBCurso",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TBProgressoModuloAluno",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatriculaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuloCursoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Concluido = table.Column<bool>(type: "bit", nullable: false),
                    DataConclusao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBProgressoModuloAluno", x => x.Id);
                    table.CheckConstraint("CK_TBProgressoModuloAluno_Conclusao", "([Concluido] = 1 AND [DataConclusao] IS NOT NULL) OR ([Concluido] = 0 AND [DataConclusao] IS NULL)");
                    table.ForeignKey(
                        name: "FK_TBProgressoModuloAluno_TBMatricula",
                        column: x => x.MatriculaId,
                        principalTable: "TBMatricula",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TBProgressoModuloAluno_TBModuloCurso",
                        column: x => x.ModuloCursoId,
                        principalTable: "TBModuloCurso",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBModuloCurso_CursoId",
                table: "TBModuloCurso",
                column: "CursoId");

            migrationBuilder.CreateIndex(
                name: "UQ_TBModuloCurso_CursoId_Ordem_Ativo",
                table: "TBModuloCurso",
                columns: new[] { "CursoId", "Ordem" },
                unique: true,
                filter: "[Ativo] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_TBProgressoModuloAluno_ModuloCursoId",
                table: "TBProgressoModuloAluno",
                column: "ModuloCursoId");

            migrationBuilder.CreateIndex(
                name: "UQ_TBProgressoModuloAluno_MatriculaId_ModuloCursoId",
                table: "TBProgressoModuloAluno",
                columns: new[] { "MatriculaId", "ModuloCursoId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBProgressoModuloAluno");

            migrationBuilder.DropTable(
                name: "TBModuloCurso");
        }
    }
}