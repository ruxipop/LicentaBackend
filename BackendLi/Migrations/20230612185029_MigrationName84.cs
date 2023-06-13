using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendLi.Migrations
{
    public partial class MigrationName84 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "gallery",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "gallery");
        }
    }
}
