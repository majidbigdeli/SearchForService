using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SearchForApi.Migrations
{
    public partial class add_feature_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Cover = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeatureItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Keyword = table.Column<string>(type: "text", nullable: true),
                    Translation = table.Column<string>(type: "text", nullable: true),
                    Sample = table.Column<string>(type: "text", nullable: true),
                    Cover = table.Column<string>(type: "text", nullable: true),
                    OwnerType = table.Column<int>(type: "integer", nullable: false),
                    OwnerObjectId = table.Column<Guid>(type: "uuid", nullable: true),
                    FeatureId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureItems_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeatureItems_FeatureId",
                table: "FeatureItems",
                column: "FeatureId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeatureItems");

            migrationBuilder.DropTable(
                name: "Features");
        }
    }
}
