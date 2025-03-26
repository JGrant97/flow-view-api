using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace flow_view.Migrations
{
    /// <inheritdoc />
    public partial class FixedFKs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AspNetUserId",
                table: "Rating",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Rating_AspNetUserId_ContentId",
                table: "Rating",
                newName: "IX_Rating_UserId_ContentId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Content",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Rating_ContentId",
                table: "Rating",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Content_UserId",
                table: "Content",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Content_AspNetUsers_UserId",
                table: "Content",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_AspNetUsers_UserId",
                table: "Rating",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Content_ContentId",
                table: "Rating",
                column: "ContentId",
                principalTable: "Content",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Content_AspNetUsers_UserId",
                table: "Content");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_AspNetUsers_UserId",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Content_ContentId",
                table: "Rating");

            migrationBuilder.DropIndex(
                name: "IX_Rating_ContentId",
                table: "Rating");

            migrationBuilder.DropIndex(
                name: "IX_Content_UserId",
                table: "Content");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Content");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Rating",
                newName: "AspNetUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Rating_UserId_ContentId",
                table: "Rating",
                newName: "IX_Rating_AspNetUserId_ContentId");
        }
    }
}
