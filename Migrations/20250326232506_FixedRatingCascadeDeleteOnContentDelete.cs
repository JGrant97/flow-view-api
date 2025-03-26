using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace flow_view.Migrations
{
    /// <inheritdoc />
    public partial class FixedRatingCascadeDeleteOnContentDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Content_ContentId",
                table: "Rating");

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Content_ContentId",
                table: "Rating",
                column: "ContentId",
                principalTable: "Content",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Content_ContentId",
                table: "Rating");

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Content_ContentId",
                table: "Rating",
                column: "ContentId",
                principalTable: "Content",
                principalColumn: "Id");
        }
    }
}
