using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WikiSistemaASP.NET.Migrations
{
    /// <inheritdoc />
    public partial class AddDescricaoToModulo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Modulos",
                newName: "Descricao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Descricao",
                table: "Modulos",
                newName: "Description");
        }
    }
}
