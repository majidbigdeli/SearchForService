using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SearchForApi.Migrations
{
    public partial class add_user_discount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountFixedAmount",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DiscountPercent",
                table: "Payments");

            migrationBuilder.AddColumn<int>(
                name: "CountPerUser",
                table: "Discounts",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "UserDiscount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DiscountCode = table.Column<string>(type: "text", nullable: true),
                    DiscountFixedAmount = table.Column<int>(type: "integer", nullable: true),
                    DiscountPercent = table.Column<int>(type: "integer", nullable: true),
                    IsFree = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDiscount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDiscount_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDiscount_Discounts_DiscountCode",
                        column: x => x.DiscountCode,
                        principalTable: "Discounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDiscount_DiscountCode",
                table: "UserDiscount",
                column: "DiscountCode");

            migrationBuilder.CreateIndex(
                name: "IX_UserDiscount_UserId",
                table: "UserDiscount",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDiscount");

            migrationBuilder.DropColumn(
                name: "CountPerUser",
                table: "Discounts");

            migrationBuilder.AddColumn<int>(
                name: "DiscountFixedAmount",
                table: "Payments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiscountPercent",
                table: "Payments",
                type: "integer",
                nullable: true);
        }
    }
}
