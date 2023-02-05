using Microsoft.EntityFrameworkCore.Migrations;

namespace SearchForApi.Migrations
{
    public partial class hits_history_info : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HitIndex",
                table: "Histories",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HitsCount",
                table: "Histories",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HitIndex",
                table: "Histories");

            migrationBuilder.DropColumn(
                name: "HitsCount",
                table: "Histories");
        }
    }
}
