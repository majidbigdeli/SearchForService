using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SearchForApi.Migrations
{
    public partial class add_movie_assets_checked : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cover",
                table: "Movies");

            migrationBuilder.AddColumn<Guid>(
                name: "AssetsCheckedByUserId",
                table: "Movies",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AssetsCheckedOn",
                table: "Movies",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssetsCheckedStatus",
                table: "Movies",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssetsCheckedByUserId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "AssetsCheckedOn",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "AssetsCheckedStatus",
                table: "Movies");

            migrationBuilder.AddColumn<string>(
                name: "Cover",
                table: "Movies",
                type: "text",
                nullable: true);
        }
    }
}
