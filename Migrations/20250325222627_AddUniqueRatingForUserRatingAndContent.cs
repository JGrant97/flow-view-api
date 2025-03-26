using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace flow_view.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueRatingForUserRatingAndContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rating_AspNetUsers_AspNetUserId",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Content_ContentId",
                table: "Rating");

            migrationBuilder.DropIndex(
                name: "IX_Rating_AspNetUserId",
                table: "Rating");

            migrationBuilder.DropIndex(
                name: "IX_Rating_ContentId",
                table: "Rating");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_AspNetUserId_ContentId",
                table: "Rating",
                columns: new[] { "AspNetUserId", "ContentId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rating_AspNetUserId_ContentId",
                table: "Rating");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_AspNetUserId",
                table: "Rating",
                column: "AspNetUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_ContentId",
                table: "Rating",
                column: "ContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_AspNetUsers_AspNetUserId",
                table: "Rating",
                column: "AspNetUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Content_ContentId",
                table: "Rating",
                column: "ContentId",
                principalTable: "Content",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
