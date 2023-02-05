using Microsoft.EntityFrameworkCore.Migrations;

namespace SearchForApi.Migrations
{
    public partial class remove_redirect_urls_from_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmationRedirectUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ResetPasswordRedirectUrl",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailConfirmationRedirectUrl",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResetPasswordRedirectUrl",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }
    }
}
