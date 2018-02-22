using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace test.Migrations
{
    public partial class fix_relations_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_PartsSupplier_PartsSupplierID1",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_PartsSupplierID1",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PartsSupplierID1",
                table: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PartsSupplierID1",
                table: "User",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_PartsSupplierID1",
                table: "User",
                column: "PartsSupplierID1");

            migrationBuilder.AddForeignKey(
                name: "FK_User_PartsSupplier_PartsSupplierID1",
                table: "User",
                column: "PartsSupplierID1",
                principalTable: "PartsSupplier",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
