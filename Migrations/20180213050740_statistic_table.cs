using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace test.Migrations
{
    public partial class statistic_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatisticCounter",
                table: "ProducerCatalogItem");

            migrationBuilder.CreateTable(
                name: "CatalogItemStatistic",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CatalogItemId = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UpdatedAt = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogItemStatistic", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CatalogItemStatistic_ProducerCatalogItem_CatalogItemId",
                        column: x => x.CatalogItemId,
                        principalTable: "ProducerCatalogItem",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItemStatistic_CatalogItemId",
                table: "CatalogItemStatistic",
                column: "CatalogItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogItemStatistic");

            migrationBuilder.AddColumn<int>(
                name: "StatisticCounter",
                table: "ProducerCatalogItem",
                nullable: false,
                defaultValue: 0);
        }
    }
}
