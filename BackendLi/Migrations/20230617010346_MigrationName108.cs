using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendLi.Migrations
{
    public partial class MigrationName108 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_LocationId",
                table: "users",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_users_location_LocationId",
                table: "users",
                column: "LocationId",
                principalTable: "location",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_location_LocationId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_LocationId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "users");
        }
    }
}
