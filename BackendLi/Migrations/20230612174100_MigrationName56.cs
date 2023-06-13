using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendLi.Migrations
{
    public partial class MigrationName56 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_image_gallery_GalleryId",
                table: "image");

            migrationBuilder.DropIndex(
                name: "IX_image_GalleryId",
                table: "image");

            migrationBuilder.DropColumn(
                name: "GalleryId",
                table: "image");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GalleryId",
                table: "image",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_image_GalleryId",
                table: "image",
                column: "GalleryId");

            migrationBuilder.AddForeignKey(
                name: "FK_image_gallery_GalleryId",
                table: "image",
                column: "GalleryId",
                principalTable: "gallery",
                principalColumn: "Id");
        }
    }
}
