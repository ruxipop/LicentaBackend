using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendLi.Migrations
{
    public partial class MigrationName81 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_image_location_LocationId",
                table: "image");

            migrationBuilder.AlterColumn<string>(
                name: "TokenValue",
                table: "token",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "image",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.AddForeignKey(
                name: "FK_image_location_LocationId",
                table: "image",
                column: "LocationId",
                principalTable: "location",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_image_gallery_GalleryId",
                table: "image");

            migrationBuilder.DropForeignKey(
                name: "FK_image_location_LocationId",
                table: "image");

            migrationBuilder.DropIndex(
                name: "IX_image_GalleryId",
                table: "image");

            migrationBuilder.DropColumn(
                name: "GalleryId",
                table: "image");

            migrationBuilder.UpdateData(
                table: "token",
                keyColumn: "TokenValue",
                keyValue: null,
                column: "TokenValue",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "TokenValue",
                table: "token",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "image",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_image_location_LocationId",
                table: "image",
                column: "LocationId",
                principalTable: "location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
