using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SearchForApi.Migrations
{
    public partial class change_scene_check : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_AspNetUsers_CheckedByUserId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Scenes_Scenes_ExcludedBySceneId",
                table: "Scenes");

            migrationBuilder.DropIndex(
                name: "IX_Reports_CheckedByUserId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "CheckedByUserId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "CheckedOn",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Result",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "ExcludedBySceneId",
                table: "Scenes",
                newName: "CheckedBySceneId");

            migrationBuilder.RenameIndex(
                name: "IX_Scenes_ExcludedBySceneId",
                table: "Scenes",
                newName: "IX_Scenes_CheckedBySceneId");

            migrationBuilder.AlterColumn<int>(
                name: "CheckType",
                table: "Scenes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "CheckResultType",
                table: "Scenes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Scenes_Scenes_CheckedBySceneId",
                table: "Scenes",
                column: "CheckedBySceneId",
                principalTable: "Scenes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scenes_Scenes_CheckedBySceneId",
                table: "Scenes");

            migrationBuilder.DropColumn(
                name: "CheckResultType",
                table: "Scenes");

            migrationBuilder.RenameColumn(
                name: "CheckedBySceneId",
                table: "Scenes",
                newName: "ExcludedBySceneId");

            migrationBuilder.RenameIndex(
                name: "IX_Scenes_CheckedBySceneId",
                table: "Scenes",
                newName: "IX_Scenes_ExcludedBySceneId");

            migrationBuilder.AlterColumn<int>(
                name: "CheckType",
                table: "Scenes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CheckedByUserId",
                table: "Reports",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckedOn",
                table: "Reports",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Result",
                table: "Reports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CheckedByUserId",
                table: "Reports",
                column: "CheckedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_AspNetUsers_CheckedByUserId",
                table: "Reports",
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
    }
}
