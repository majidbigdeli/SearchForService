using Microsoft.EntityFrameworkCore.Migrations;

namespace SearchForApi.Migrations
{
    public partial class history_refer_type : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReferType",
                table: "Histories",
                type: "integer",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferType",
                table: "Histories");
        }
    }
}
