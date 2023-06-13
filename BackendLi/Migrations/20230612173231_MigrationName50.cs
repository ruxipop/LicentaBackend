using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendLi.Migrations
{
    public partial class MigrationName50 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GalleryId",
                table: "image",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "gallery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gallery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_gallery_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_image_GalleryId",
                table: "image",
                column: "GalleryId");

            migrationBuilder.CreateIndex(
                name: "IX_gallery_UserId",
                table: "gallery",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_image_gallery_GalleryId",
                table: "image",
                column: "GalleryId",
                principalTable: "gallery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_image_gallery_GalleryId",
                table: "image");

            migrationBuilder.DropTable(
                name: "gallery");

            migrationBuilder.DropIndex(
                name: "IX_image_GalleryId",
                table: "image");

            migrationBuilder.DropColumn(
                name: "GalleryId",
                table: "image");
        }
    }
}
