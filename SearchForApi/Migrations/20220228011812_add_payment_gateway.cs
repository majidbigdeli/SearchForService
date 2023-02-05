using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SearchForApi.Migrations
{
    public partial class add_payment_gateway : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GatewayType",
                table: "Payments",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "PaymentGateway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    IsEnable = table.Column<bool>(type: "boolean", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_Movies_AssetsCheckedByUserId",
                table: "Movies",
                column: "AssetsCheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentGateway_ModifiedByUserId",
                table: "PaymentGateway",
                column: "ModifiedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_AspNetUsers_AssetsCheckedByUserId",
                table: "Movies",
                column: "AssetsCheckedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_AspNetUsers_AssetsCheckedByUserId",
                table: "Movies");

            migrationBuilder.DropTable(
                name: "PaymentGateway");

            migrationBuilder.DropIndex(
                name: "IX_Movies_AssetsCheckedByUserId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "GatewayType",
                table: "Payments");
        }
    }
}
