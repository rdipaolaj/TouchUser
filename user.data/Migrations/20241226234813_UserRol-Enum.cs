using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace user.data.Migrations
{
    /// <inheritdoc />
    public partial class UserRolEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "usersch",
                table: "Roles");

            migrationBuilder.AddColumn<int>(
                name: "userRole",
                schema: "usersch",
                table: "Roles",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userRole",
                schema: "usersch",
                table: "Roles");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "usersch",
                table: "Roles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
