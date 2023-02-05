using Microsoft.EntityFrameworkCore.Migrations;

namespace SearchForApi.Migrations
{
    public partial class add_version_app_release : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VersionBuild",
                table: "AppReleases",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VersionMajor",
                table: "AppReleases",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VersionMinor",
                table: "AppReleases",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VersionRevision",
                table: "AppReleases",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VersionBuild",
                table: "AppReleases");

            migrationBuilder.DropColumn(
                name: "VersionMajor",
                table: "AppReleases");

            migrationBuilder.DropColumn(
                name: "VersionMinor",
                table: "AppReleases");

            migrationBuilder.DropColumn(
                name: "VersionRevision",
                table: "AppReleases");
        }
    }
}
