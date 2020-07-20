using System;
using System.Collections.Generic;
using FanFiction.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FanFiction.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            


            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    GenreName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.GenreName);
                });


            migrationBuilder.CreateTable(
                name: "Fictions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Rating = table.Column<double>(nullable: false),
                    UploadDate = table.Column<DateTime>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    AuthorId = table.Column<string>(nullable: true),
                    Genre = table.Column<string>(nullable: true),
                    RatesCount = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fictions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fictions_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

         
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Tags");


            migrationBuilder.DropTable(
                name: "Fictions");

        }
    }
}
