using Microsoft.EntityFrameworkCore.Migrations;

namespace SearchForApi.Migrations
{
    public partial class add_user_storage_id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StorageId",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StorageId",
                table: "AspNetUsers");
        }
    }
}
