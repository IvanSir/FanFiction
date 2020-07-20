using Microsoft.EntityFrameworkCore.Migrations;

namespace FanFiction.Migrations
{
    public partial class ratata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Rates");

            migrationBuilder.AddColumn<string>(
                name: "FictionId",
                table: "Rates",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Rates",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FictionId",
                table: "Rates");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Rates");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Rates",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
