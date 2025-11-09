using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scratch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Projects",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_DeletedAt",
                table: "Projects",
                column: "DeletedAt",
                filter: "\"DeletedAt\" IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Projects_DeletedAt",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Projects");
        }
    }
}
