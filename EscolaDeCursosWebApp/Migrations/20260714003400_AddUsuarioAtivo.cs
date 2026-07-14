using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscolaDeCursosWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddUsuarioAtivo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ativo",
                table: "TBUsuario",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ativo",
                table: "TBUsuario");
        }
    }
}
