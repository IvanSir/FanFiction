using Microsoft.EntityFrameworkCore.Migrations;

namespace FanFiction.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chapters",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    FictionId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: false),
                    ChapterNumber = table.Column<int>(nullable: false),
                    ChapterText = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chapters_Fictions_FictionId",
                        column: x => x.FictionId,
                        principalTable: "Fictions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_FictionId",
                table: "Chapters",
                column: "FictionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chapters");
        }
    }
}
