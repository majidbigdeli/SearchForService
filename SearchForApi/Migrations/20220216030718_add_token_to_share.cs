using Microsoft.EntityFrameworkCore.Migrations;

namespace SearchForApi.Migrations
{
    public partial class add_token_to_share : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Shares",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex("IX_Share_Token", "Shares", "Token", unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex("IX_Share_Token", "Shares");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Shares");
        }
    }
}
