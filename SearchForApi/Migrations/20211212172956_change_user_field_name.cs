using Microsoft.EntityFrameworkCore.Migrations;

namespace SearchForApi.Migrations
{
    public partial class change_user_field_name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpireDate",
                table: "AspNetUsers",
                newName: "PlanExpireDate");

            migrationBuilder.AlterColumn<float>(
                name: "ImdbScore",
                table: "Movies",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlanExpireDate",
                table: "AspNetUsers",
                newName: "ExpireDate");

            migrationBuilder.AlterColumn<float>(
                name: "ImdbScore",
                table: "Movies",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);
        }
    }
}
