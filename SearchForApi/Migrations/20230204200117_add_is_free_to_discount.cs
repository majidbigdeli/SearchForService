using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SearchForApi.Migrations
{
    public partial class add_is_free_to_discount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFree",
                table: "Discounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFree",
                table: "Discounts");
        }
    }
}
