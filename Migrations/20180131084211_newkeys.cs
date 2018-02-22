using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace test.Migrations {
    public partial class newkeys : Migration {
        protected override void Up (MigrationBuilder migrationBuilder) {
            migrationBuilder.DropForeignKey (
                name: "FK_SupplierPriceItem_PartProducer_ProducerId",
                table: "SupplierPriceItem");

            migrationBuilder.Sql ("SET FOREIGN_KEY_CHECKS=0;" +
                "ALTER TABLE `SupplierPriceItem` DROP INDEX `AK_SupplierPriceItem_PartsSupplierId_ProducerCodeTrimmed`;" +
                "SET FOREIGN_KEY_CHECKS=1;"
            );

            migrationBuilder.AlterColumn<int> (
                name: "ProducerId",
                table: "SupplierPriceItem",
                nullable : false,
                oldClrType : typeof (int),
                oldNullable : true);

            migrationBuilder.AddUniqueConstraint (
                name: "AK_SupplierPriceItem_PartsSupId_ProdId_ProdCodeTr",
                table: "SupplierPriceItem",
                columns : new [] { "PartsSupplierId", "ProducerId", "ProducerCodeTrimmed" });

            migrationBuilder.AddForeignKey (
                name: "FK_SupplierPriceItem_PartProducer_ProducerId",
                table: "SupplierPriceItem",
                column: "ProducerId",
                principalTable: "PartProducer",
                principalColumn: "ID",
                onDelete : ReferentialAction.Cascade);
        }

        protected override void Down (MigrationBuilder migrationBuilder) {
            migrationBuilder.DropForeignKey (
                name: "FK_SupplierPriceItem_PartProducer_ProducerId",
                table: "SupplierPriceItem");

            migrationBuilder.Sql ("SET FOREIGN_KEY_CHECKS=0;" +
                "ALTER TABLE `SupplierPriceItem` DROP INDEX `AK_SupplierPriceItem_PartsSupId_ProdId_ProdCodeTr`;" +
                "SET FOREIGN_KEY_CHECKS=1;"
            );

            migrationBuilder.AlterColumn<int> (
                name: "ProducerId",
                table: "SupplierPriceItem",
                nullable : true,
                oldClrType : typeof (int));

            migrationBuilder.AddUniqueConstraint (
                name: "AK_SupplierPriceItem_PartsSupplierId_ProducerCodeTrimmed",
                table: "SupplierPriceItem",
                columns : new [] { "PartsSupplierId", "ProducerCodeTrimmed" });

            migrationBuilder.AddForeignKey (
                name: "FK_SupplierPriceItem_PartProducer_ProducerId",
                table: "SupplierPriceItem",
                column: "ProducerId",
                principalTable: "PartProducer",
                principalColumn: "ID",
                onDelete : ReferentialAction.Restrict);
        }
    }
}