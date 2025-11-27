using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Obligatorio.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class Nueva2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Discriminator",
                table: "Pagos",
                newName: "PagoTipo");

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "TipoGastos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Fecha",
                table: "Pagos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activo",
                table: "TipoGastos");

            migrationBuilder.DropColumn(
                name: "Fecha",
                table: "Pagos");

            migrationBuilder.RenameColumn(
                name: "PagoTipo",
                table: "Pagos",
                newName: "Discriminator");
        }
    }
}
