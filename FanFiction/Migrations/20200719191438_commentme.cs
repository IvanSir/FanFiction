using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FanFiction.Migrations
{
    public partial class commentme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    CommentDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    ChapterId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ChapterId",
                table: "Comments",
                column: "ChapterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");
        }
    }
}
