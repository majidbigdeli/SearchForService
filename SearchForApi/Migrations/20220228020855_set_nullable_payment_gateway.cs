using Microsoft.EntityFrameworkCore.Migrations;

namespace SearchForApi.Migrations
{
    public partial class set_nullable_payment_gateway : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentGateway_GatewayType",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "GatewayType",
                table: "Payments",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentGateway_GatewayType",
                table: "Payments",
                column: "GatewayType",
                principalTable: "PaymentGateway",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentGateway_GatewayType",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "GatewayType",
                table: "Payments",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentGateway_GatewayType",
                table: "Payments",
                column: "GatewayType",
                principalTable: "PaymentGateway",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
