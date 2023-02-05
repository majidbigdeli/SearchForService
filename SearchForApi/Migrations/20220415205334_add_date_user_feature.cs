using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SearchForApi.Migrations
{
    public partial class add_date_user_feature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerType",
                table: "FeatureItems");

            migrationBuilder.RenameColumn(
                name: "OwnerObjectId",
                table: "FeatureItems",
                newName: "ParameterId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Features",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ChangedByUserId",
                table: "FeatureItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("9eeb8fc3-2ce3-4e11-a5cc-153bf4ee7dca"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangedOn",
                table: "FeatureItems",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "FeatureItems",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_FeatureItems_ChangedByUserId",
                table: "FeatureItems",
                column: "ChangedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureItems_AspNetUsers_ChangedByUserId",
                table: "FeatureItems",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureItems_AspNetUsers_ChangedByUserId",
                table: "FeatureItems");

            migrationBuilder.DropIndex(
                name: "IX_FeatureItems_ChangedByUserId",
                table: "FeatureItems");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "ChangedByUserId",
                table: "FeatureItems");

            migrationBuilder.DropColumn(
                name: "ChangedOn",
                table: "FeatureItems");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "FeatureItems");

            migrationBuilder.RenameColumn(
                name: "ParameterId",
                table: "FeatureItems",
                newName: "OwnerObjectId");

            migrationBuilder.AddColumn<int>(
                name: "OwnerType",
                table: "FeatureItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
