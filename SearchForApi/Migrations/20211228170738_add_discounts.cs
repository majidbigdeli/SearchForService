using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SearchForApi.Migrations
{
    public partial class add_discounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Payments",
                newName: "PlanPrice");

            migrationBuilder.RenameColumn(
                name: "Is3Months",
                table: "Payments",
                newName: "PlanIs3Months");

            migrationBuilder.RenameColumn(
                name: "DiscountPrecentFor3Months",
                table: "Payments",
                newName: "PlanDiscountPrecentFor3Months");

            migrationBuilder.RenameColumn(
                name: "Discount",
                table: "Payments",
                newName: "PlanDiscount");

            migrationBuilder.AddColumn<string>(
                name: "DiscountCode",
                table: "Payments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiscountFixedAmount",
                table: "Payments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DiscountPercent",
                table: "Payments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Max = table.Column<int>(type: "integer", nullable: false),
                    Used = table.Column<int>(type: "integer", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    FixedAmount = table.Column<int>(type: "integer", nullable: false),
                    Percent = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_DiscountCode",
                table: "Payments",
                column: "DiscountCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Discounts_DiscountCode",
                table: "Payments",
                column: "DiscountCode",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Discounts_DiscountCode",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropIndex(
                name: "IX_Payments_DiscountCode",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DiscountCode",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DiscountFixedAmount",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DiscountPercent",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "PlanPrice",
                table: "Payments",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "PlanIs3Months",
                table: "Payments",
                newName: "Is3Months");

            migrationBuilder.RenameColumn(
                name: "PlanDiscountPrecentFor3Months",
                table: "Payments",
                newName: "DiscountPrecentFor3Months");

            migrationBuilder.RenameColumn(
                name: "PlanDiscount",
                table: "Payments",
                newName: "Discount");
        }
    }
}
