using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scratch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TokenLamMoi",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    TokenLamMoi_ThoiHan = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IdNguoiDung = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenLamMoi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TokenLamMoi_NguoiDung_IdNguoiDung",
                        column: x => x.IdNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TokenLamMoi_IdNguoiDung",
                table: "TokenLamMoi",
                column: "IdNguoiDung");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TokenLamMoi");
        }
    }
}
