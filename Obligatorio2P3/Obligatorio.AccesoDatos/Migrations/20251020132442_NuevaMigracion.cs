using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Obligatorio.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class NuevaMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Contenidos_EquipoId",
                table: "Usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contenidos",
                table: "Contenidos");

            migrationBuilder.DropColumn(
                name: "CantidadOperacionesRealizadas",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "LimiteAprobacionPagos",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "MontoMaximoPagoSinAprobacion",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "UltimaAuditoriaRealizada",
                table: "Usuarios");

            migrationBuilder.RenameTable(
                name: "Contenidos",
                newName: "Equipos");

            migrationBuilder.RenameColumn(
                name: "Contraseña",
                table: "Usuarios",
                newName: "Contrasenia");

            migrationBuilder.AddColumn<int>(
                name: "Rol",
                table: "Usuarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Equipos",
                table: "Equipos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Equipos_EquipoId",
                table: "Usuarios",
                column: "EquipoId",
                principalTable: "Equipos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Equipos_EquipoId",
                table: "Usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Equipos",
                table: "Equipos");

            migrationBuilder.DropColumn(
                name: "Rol",
                table: "Usuarios");

            migrationBuilder.RenameTable(
                name: "Equipos",
                newName: "Contenidos");

            migrationBuilder.RenameColumn(
                name: "Contrasenia",
                table: "Usuarios",
                newName: "Contraseña");

            migrationBuilder.AddColumn<int>(
                name: "CantidadOperacionesRealizadas",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Usuarios",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Usuarios",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "LimiteAprobacionPagos",
                table: "Usuarios",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MontoMaximoPagoSinAprobacion",
                table: "Usuarios",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UltimaAuditoriaRealizada",
                table: "Usuarios",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contenidos",
                table: "Contenidos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Contenidos_EquipoId",
                table: "Usuarios",
                column: "EquipoId",
                principalTable: "Contenidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
