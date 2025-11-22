using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scratch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCommentRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "ProjectComments",
                newName: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectComments_ParentCommentId",
                table: "ProjectComments",
                column: "ParentCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectComments_ProjectComments_ParentCommentId",
                table: "ProjectComments",
                column: "ParentCommentId",
                principalTable: "ProjectComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectComments_ProjectComments_ParentCommentId",
                table: "ProjectComments");

            migrationBuilder.DropIndex(
                name: "IX_ProjectComments_ParentCommentId",
                table: "ProjectComments");

            migrationBuilder.RenameColumn(
                name: "ParentCommentId",
                table: "ProjectComments",
                newName: "ParentId");
        }
    }
}
