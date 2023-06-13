using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendLi.Migrations
{
    public partial class MigrationName52 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_image_gallery_GalleryId",
                table: "image");

            migrationBuilder.AlterColumn<int>(
                name: "GalleryId",
                table: "image",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_image_gallery_GalleryId",
                table: "image",
                column: "GalleryId",
                principalTable: "gallery",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_image_gallery_GalleryId",
                table: "image");

            migrationBuilder.AlterColumn<int>(
                name: "GalleryId",
                table: "image",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_image_gallery_GalleryId",
                table: "image",
                column: "GalleryId",
                principalTable: "gallery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
