using Microsoft.EntityFrameworkCore.Migrations;

namespace FanFiction.Migrations
{
    public partial class uptag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FicId",
                table: "Tags",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FicId",
                table: "Tags");
        }
    }
}
