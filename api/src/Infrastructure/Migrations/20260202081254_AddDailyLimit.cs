using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scratch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDailyLimit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserDailyUploadStats",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    UploadCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDailyUploadStats", x => new { x.UserId, x.Date });
                    table.ForeignKey(
                        name: "FK_UserDailyUploadStats_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDailyUploadStats");
        }
    }
}
