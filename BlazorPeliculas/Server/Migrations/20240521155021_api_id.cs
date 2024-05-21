using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorPeliculas.Server.Migrations
{
    /// <inheritdoc />
    public partial class api_id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaLanzamiento",
                table: "Peliculas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "APIId",
                table: "Peliculas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "APIId",
                table: "Generos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "APIId",
                table: "Actores",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "APIId",
                table: "Peliculas");

            migrationBuilder.DropColumn(
                name: "APIId",
                table: "Generos");

            migrationBuilder.DropColumn(
                name: "APIId",
                table: "Actores");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaLanzamiento",
                table: "Peliculas",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
