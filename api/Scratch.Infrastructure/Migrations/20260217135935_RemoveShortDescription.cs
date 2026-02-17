using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scratch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveShortDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "Projects");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "Projects",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
