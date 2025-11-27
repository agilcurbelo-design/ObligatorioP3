using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Obligatorio.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class PorLasDudas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Fecha",
                table: "Auditorias",
                newName: "FechaOperacion");

            migrationBuilder.RenameColumn(
                name: "Accion",
                table: "Auditorias",
                newName: "TipoOperacion");

            migrationBuilder.AddColumn<string>(
                name: "NombreTipoGasto",
                table: "Auditorias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NombreTipoGasto",
                table: "Auditorias");

            migrationBuilder.RenameColumn(
                name: "TipoOperacion",
                table: "Auditorias",
                newName: "Accion");

            migrationBuilder.RenameColumn(
                name: "FechaOperacion",
                table: "Auditorias",
                newName: "Fecha");
        }
    }
}
