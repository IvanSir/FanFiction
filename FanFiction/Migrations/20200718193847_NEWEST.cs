using Microsoft.EntityFrameworkCore.Migrations;

namespace FanFiction.Migrations
{
    public partial class NEWEST : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
   

            migrationBuilder.CreateIndex(
                name: "IX_Tags_FictionId",
                table: "Tags",
                column: "FictionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Fictions_FictionId",
                table: "Tags",
                column: "FictionId",
                principalTable: "Fictions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Fictions_FictionId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_FictionId",
                table: "Tags");


        }
    }
}
