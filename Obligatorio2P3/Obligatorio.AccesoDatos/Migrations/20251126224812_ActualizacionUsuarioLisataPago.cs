using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Obligatorio.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class ActualizacionUsuarioLisataPago : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Discriminator",
                table: "Pagos",
                newName: "PagoTipo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PagoTipo",
                table: "Pagos",
                newName: "Discriminator");
        }
    }
}
