using Microsoft.EntityFrameworkCore.Migrations;

namespace FanFiction.Migrations
{
    public partial class nemiga1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rate_AspNetUsers_ApplicationUserId",
                table: "Rate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rate",
                table: "Rate");

            migrationBuilder.RenameTable(
                name: "Rate",
                newName: "Rates");

            migrationBuilder.RenameIndex(
                name: "IX_Rate_ApplicationUserId",
                table: "Rates",
                newName: "IX_Rates_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rates",
                table: "Rates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_AspNetUsers_ApplicationUserId",
                table: "Rates",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rates_AspNetUsers_ApplicationUserId",
                table: "Rates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rates",
                table: "Rates");

            migrationBuilder.RenameTable(
                name: "Rates",
                newName: "Rate");

            migrationBuilder.RenameIndex(
                name: "IX_Rates_ApplicationUserId",
                table: "Rate",
                newName: "IX_Rate_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rate",
                table: "Rate",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rate_AspNetUsers_ApplicationUserId",
                table: "Rate",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
