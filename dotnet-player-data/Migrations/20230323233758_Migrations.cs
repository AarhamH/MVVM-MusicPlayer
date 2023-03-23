using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_player_data.Migrations
{
    /// <inheritdoc />
    public partial class Migrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayListObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PLTitle = table.Column<string>(type: "TEXT", maxLength: 40, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayListObjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SongObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Path = table.Column<string>(type: "TEXT", nullable: true),
                    ListID = table.Column<int>(type: "INTEGER", nullable: true),
                    PlayListId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SongObjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SongObjects_PlayListObjects_PlayListId",
                        column: x => x.PlayListId,
                        principalTable: "PlayListObjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SongObjects_PlayListId",
                table: "SongObjects",
                column: "PlayListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SongObjects");

            migrationBuilder.DropTable(
                name: "PlayListObjects");
        }
    }
}
