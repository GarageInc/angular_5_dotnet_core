using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace test.Migrations
{
    public partial class files_supplier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PartsSupplierID",
                table: "SupplierOfferFile",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupplierOfferFile_PartsSupplierID",
                table: "SupplierOfferFile",
                column: "PartsSupplierID");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierOfferFile_PartsSupplier_PartsSupplierID",
                table: "SupplierOfferFile",
                column: "PartsSupplierID",
                principalTable: "PartsSupplier",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplierOfferFile_PartsSupplier_PartsSupplierID",
                table: "SupplierOfferFile");

            migrationBuilder.DropIndex(
                name: "IX_SupplierOfferFile_PartsSupplierID",
                table: "SupplierOfferFile");

            migrationBuilder.DropColumn(
                name: "PartsSupplierID",
                table: "SupplierOfferFile");
        }
    }
}
