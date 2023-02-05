using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SearchForApi.Migrations
{
    public partial class minor_changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Languages_LanguageId",
                table: "Movies");

            migrationBuilder.AlterColumn<int>(
                name: "LanguageId",
                table: "Movies",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "IntroStart",
                table: "Movies",
                type: "interval",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "interval");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "IntroEnd",
                table: "Movies",
                type: "interval",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "interval");


            migrationBuilder.Sql("ALTER TABLE \"Movies\" ALTER COLUMN \"ImdbScore\" TYPE real USING \"ImdbScore\"::real;");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Languages_LanguageId",
                table: "Movies",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Languages_LanguageId",
                table: "Movies");

            migrationBuilder.AlterColumn<int>(
                name: "LanguageId",
                table: "Movies",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "IntroStart",
                table: "Movies",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "interval",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "IntroEnd",
                table: "Movies",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "interval",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImdbScore",
                table: "Movies",
                type: "text",
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Languages_LanguageId",
                table: "Movies",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
