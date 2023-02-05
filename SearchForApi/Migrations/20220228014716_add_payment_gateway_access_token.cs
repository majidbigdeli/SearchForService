using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SearchForApi.Migrations
{
    public partial class add_payment_gateway_access_token : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("PaymentGateway");

            migrationBuilder.CreateTable(
                name: "PaymentGateway",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    IsEnable = table.Column<bool>(type: "boolean", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    AccessToken = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentGateway", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentGateway_AspNetUsers_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData("PaymentGateway", new[] { "Id", "IsEnable", "Order", "AccessToken", "CreatedOn", "ModifiedOn", "ModifiedByUserId" }, new object[] { 1, true, 1, "", DateTime.UtcNow, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_GatewayType",
                table: "Payments",
                column: "GatewayType");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentGateway_GatewayType",
                table: "Payments",
                column: "GatewayType",
                principalTable: "PaymentGateway",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
