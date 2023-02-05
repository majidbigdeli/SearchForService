using Microsoft.EntityFrameworkCore.Migrations;

namespace SearchForApi.Migrations
{
    public partial class add_enable_discount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Max",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "Used",
                table: "Discounts");

            migrationBuilder.AddColumn<bool>(
                name: "IsEnable",
                table: "Discounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnable",
                table: "Discounts");

            migrationBuilder.AddColumn<int>(
                name: "Max",
                table: "Discounts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Used",
                table: "Discounts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
