using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorPeliculas.Server.Migrations
{
    /// <inheritdoc />
    public partial class RolAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO AspNetRoles(Id, Name, NormalizedName)
                                    VALUES('f40279bf-3b55-4aa0-a7c4-c337afb1a9f0', 'admin', 'ADMIN')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE AspNetRoles WHERE Id = 'f40279bf-3b55-4aa0-a7c4-c337afb1a9f0'");

		}
    }
}
