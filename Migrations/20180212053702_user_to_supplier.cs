using System;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections.Generic;

namespace test.Migrations {
    public partial class user_to_supplier : Migration {
        protected override void Up (MigrationBuilder migrationBuilder) {
            migrationBuilder.AddColumn<int> (
                name: "PartsSupplierID",
                table: "User",
                nullable : true);

            migrationBuilder.AddColumn<int> (
                name: "PartsSupplierID1",
                table: "User",
                nullable : true);

            migrationBuilder.CreateIndex (
                name: "IX_User_PartsSupplierID",
                table: "User",
                column: "PartsSupplierID");

            migrationBuilder.CreateIndex (
                name: "IX_User_PartsSupplierID1",
                table: "User",
                column: "PartsSupplierID1");

            migrationBuilder.AddForeignKey (
                name: "FK_User_PartsSupplier_PartsSupplierID",
                table: "User",
                column: "PartsSupplierID",
                principalTable: "PartsSupplier",
                principalColumn: "ID",
                onDelete : ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey (
                name: "FK_User_PartsSupplier_PartsSupplierID1",
                table: "User",
                column: "PartsSupplierID1",
                principalTable: "PartsSupplier",
                principalColumn: "ID",
                onDelete : ReferentialAction.Restrict);
        }

        protected override void Down (MigrationBuilder migrationBuilder) {
            migrationBuilder.DropForeignKey (
                name: "FK_User_PartsSupplier_PartsSupplierID",
                table: "User");

            migrationBuilder.DropForeignKey (
                name: "FK_User_PartsSupplier_PartsSupplierID1",
                table: "User");

            migrationBuilder.DropIndex (
                name: "IX_User_PartsSupplierID",
                table: "User");

            migrationBuilder.DropIndex (
                name: "IX_User_PartsSupplierID1",
                table: "User");

            migrationBuilder.DropColumn (
                name: "PartsSupplierID",
                table: "User");

            migrationBuilder.DropColumn (
                name: "PartsSupplierID1",
                table: "User");

        }
    }
}