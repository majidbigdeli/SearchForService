using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SearchForApi.Migrations
{
    public partial class limit_scenes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CheckExcludeType",
                table: "Scenes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CheckType",
                table: "Scenes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "CheckedByUserId",
                table: "Scenes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckedOn",
                table: "Scenes",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExcludedBySceneId",
                table: "Scenes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scenes_CheckedByUserId",
                table: "Scenes",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Scenes_ExcludedBySceneId",
                table: "Scenes",
                column: "ExcludedBySceneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scenes_AspNetUsers_CheckedByUserId",
                table: "Scenes",
                column: "CheckedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Scenes_Scenes_ExcludedBySceneId",
                table: "Scenes",
                column: "ExcludedBySceneId",
                principalTable: "Scenes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scenes_AspNetUsers_CheckedByUserId",
                table: "Scenes");

            migrationBuilder.DropForeignKey(
                name: "FK_Scenes_Scenes_ExcludedBySceneId",
                table: "Scenes");

            migrationBuilder.DropIndex(
                name: "IX_Scenes_CheckedByUserId",
                table: "Scenes");

            migrationBuilder.DropIndex(
                name: "IX_Scenes_ExcludedBySceneId",
                table: "Scenes");

            migrationBuilder.DropColumn(
                name: "CheckExcludeType",
                table: "Scenes");

            migrationBuilder.DropColumn(
                name: "CheckType",
                table: "Scenes");

            migrationBuilder.DropColumn(
                name: "CheckedByUserId",
                table: "Scenes");

            migrationBuilder.DropColumn(
                name: "CheckedOn",
                table: "Scenes");

            migrationBuilder.DropColumn(
                name: "ExcludedBySceneId",
                table: "Scenes");
        }
    }
}
