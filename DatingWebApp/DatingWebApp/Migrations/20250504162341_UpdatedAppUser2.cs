using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingWebApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedAppUser2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Intoduction",
                table: "Users",
                newName: "Introduction");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Introduction",
                table: "Users",
                newName: "Intoduction");
        }
    }
}
