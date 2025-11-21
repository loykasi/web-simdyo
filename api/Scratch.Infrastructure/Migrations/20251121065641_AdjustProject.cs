using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scratch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdjustProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBanned",
                table: "Projects");

            migrationBuilder.AddColumn<Guid>(
                name: "RevokedByUserId",
                table: "UserBans",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserBans_RevokedByUserId",
                table: "UserBans",
                column: "RevokedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBans_AspNetUsers_RevokedByUserId",
                table: "UserBans",
                column: "RevokedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBans_AspNetUsers_RevokedByUserId",
                table: "UserBans");

            migrationBuilder.DropIndex(
                name: "IX_UserBans_RevokedByUserId",
                table: "UserBans");

            migrationBuilder.DropColumn(
                name: "RevokedByUserId",
                table: "UserBans");

            migrationBuilder.AddColumn<bool>(
                name: "IsBanned",
                table: "Projects",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
