using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace test.Migrations
{
    public partial class new_user_scheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_User_CreatedByID",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Address_User_UpdatedByID",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Contact_User_CreatedByID",
                table: "Contact");

            migrationBuilder.DropForeignKey(
                name: "FK_Contact_User_UpdatedByID",
                table: "Contact");

            migrationBuilder.DropForeignKey(
                name: "FK_PartProducer_User_CreatedByID",
                table: "PartProducer");

            migrationBuilder.DropForeignKey(
                name: "FK_PartProducer_User_UpdatedByID",
                table: "PartProducer");

            migrationBuilder.DropForeignKey(
                name: "FK_SalePoint_User_CreatedByID",
                table: "SalePoint");

            migrationBuilder.DropForeignKey(
                name: "FK_SalePoint_User_UpdatedByID",
                table: "SalePoint");

            migrationBuilder.DropForeignKey(
                name: "FK_SeoParameter_User_CreatedByID",
                table: "SeoParameter");

            migrationBuilder.DropForeignKey(
                name: "FK_SeoParameter_User_UpdatedByID",
                table: "SeoParameter");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierOfferFile_User_UserID",
                table: "SupplierOfferFile");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierWarehouse_User_CreatedByID",
                table: "SupplierWarehouse");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierWarehouse_User_UpdatedByID",
                table: "SupplierWarehouse");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "User",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Login",
                table: "User",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "UpdatedByID",
                table: "SupplierWarehouse",
                newName: "UpdatedById");

            migrationBuilder.RenameColumn(
                name: "CreatedByID",
                table: "SupplierWarehouse",
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_SupplierWarehouse_UpdatedByID",
                table: "SupplierWarehouse",
                newName: "IX_SupplierWarehouse_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_SupplierWarehouse_CreatedByID",
                table: "SupplierWarehouse",
                newName: "IX_SupplierWarehouse_CreatedById");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "SupplierOfferFile",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SupplierOfferFile_UserID",
                table: "SupplierOfferFile",
                newName: "IX_SupplierOfferFile_UserId");

            migrationBuilder.RenameColumn(
                name: "UpdatedByID",
                table: "SeoParameter",
                newName: "UpdatedById");

            migrationBuilder.RenameColumn(
                name: "CreatedByID",
                table: "SeoParameter",
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_SeoParameter_UpdatedByID",
                table: "SeoParameter",
                newName: "IX_SeoParameter_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_SeoParameter_CreatedByID",
                table: "SeoParameter",
                newName: "IX_SeoParameter_CreatedById");

            migrationBuilder.RenameColumn(
                name: "UpdatedByID",
                table: "SalePoint",
                newName: "UpdatedById");

            migrationBuilder.RenameColumn(
                name: "CreatedByID",
                table: "SalePoint",
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_SalePoint_UpdatedByID",
                table: "SalePoint",
                newName: "IX_SalePoint_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_SalePoint_CreatedByID",
                table: "SalePoint",
                newName: "IX_SalePoint_CreatedById");

            migrationBuilder.RenameColumn(
                name: "UpdatedByID",
                table: "PartProducer",
                newName: "UpdatedById");

            migrationBuilder.RenameColumn(
                name: "CreatedByID",
                table: "PartProducer",
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_PartProducer_UpdatedByID",
                table: "PartProducer",
                newName: "IX_PartProducer_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_PartProducer_CreatedByID",
                table: "PartProducer",
                newName: "IX_PartProducer_CreatedById");

            migrationBuilder.RenameColumn(
                name: "UpdatedByID",
                table: "Contact",
                newName: "UpdatedById");

            migrationBuilder.RenameColumn(
                name: "CreatedByID",
                table: "Contact",
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Contact_UpdatedByID",
                table: "Contact",
                newName: "IX_Contact_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Contact_CreatedByID",
                table: "Contact",
                newName: "IX_Contact_CreatedById");

            migrationBuilder.RenameColumn(
                name: "UpdatedByID",
                table: "Address",
                newName: "UpdatedById");

            migrationBuilder.RenameColumn(
                name: "CreatedByID",
                table: "Address",
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Address_UpdatedByID",
                table: "Address",
                newName: "IX_Address_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Address_CreatedByID",
                table: "Address",
                newName: "IX_Address_CreatedById");

            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "User",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "User",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "User",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "User",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "User",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Role",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Role",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Address_User_CreatedById",
                table: "Address",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Address_User_UpdatedById",
                table: "Address",
                column: "UpdatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_User_CreatedById",
                table: "Contact",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_User_UpdatedById",
                table: "Contact",
                column: "UpdatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PartProducer_User_CreatedById",
                table: "PartProducer",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PartProducer_User_UpdatedById",
                table: "PartProducer",
                column: "UpdatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalePoint_User_CreatedById",
                table: "SalePoint",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalePoint_User_UpdatedById",
                table: "SalePoint",
                column: "UpdatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SeoParameter_User_CreatedById",
                table: "SeoParameter",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SeoParameter_User_UpdatedById",
                table: "SeoParameter",
                column: "UpdatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierOfferFile_User_UserId",
                table: "SupplierOfferFile",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierWarehouse_User_CreatedById",
                table: "SupplierWarehouse",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierWarehouse_User_UpdatedById",
                table: "SupplierWarehouse",
                column: "UpdatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_User_CreatedById",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Address_User_UpdatedById",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Contact_User_CreatedById",
                table: "Contact");

            migrationBuilder.DropForeignKey(
                name: "FK_Contact_User_UpdatedById",
                table: "Contact");

            migrationBuilder.DropForeignKey(
                name: "FK_PartProducer_User_CreatedById",
                table: "PartProducer");

            migrationBuilder.DropForeignKey(
                name: "FK_PartProducer_User_UpdatedById",
                table: "PartProducer");

            migrationBuilder.DropForeignKey(
                name: "FK_SalePoint_User_CreatedById",
                table: "SalePoint");

            migrationBuilder.DropForeignKey(
                name: "FK_SalePoint_User_UpdatedById",
                table: "SalePoint");

            migrationBuilder.DropForeignKey(
                name: "FK_SeoParameter_User_CreatedById",
                table: "SeoParameter");

            migrationBuilder.DropForeignKey(
                name: "FK_SeoParameter_User_UpdatedById",
                table: "SeoParameter");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierOfferFile_User_UserId",
                table: "SupplierOfferFile");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierWarehouse_User_CreatedById",
                table: "SupplierWarehouse");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierWarehouse_User_UpdatedById",
                table: "SupplierWarehouse");

            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "User");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "User");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "User");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "User");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Role");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "User",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "User",
                newName: "Login");

            migrationBuilder.RenameColumn(
                name: "UpdatedById",
                table: "SupplierWarehouse",
                newName: "UpdatedByID");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "SupplierWarehouse",
                newName: "CreatedByID");

            migrationBuilder.RenameIndex(
                name: "IX_SupplierWarehouse_UpdatedById",
                table: "SupplierWarehouse",
                newName: "IX_SupplierWarehouse_UpdatedByID");

            migrationBuilder.RenameIndex(
                name: "IX_SupplierWarehouse_CreatedById",
                table: "SupplierWarehouse",
                newName: "IX_SupplierWarehouse_CreatedByID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "SupplierOfferFile",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_SupplierOfferFile_UserId",
                table: "SupplierOfferFile",
                newName: "IX_SupplierOfferFile_UserID");

            migrationBuilder.RenameColumn(
                name: "UpdatedById",
                table: "SeoParameter",
                newName: "UpdatedByID");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "SeoParameter",
                newName: "CreatedByID");

            migrationBuilder.RenameIndex(
                name: "IX_SeoParameter_UpdatedById",
                table: "SeoParameter",
                newName: "IX_SeoParameter_UpdatedByID");

            migrationBuilder.RenameIndex(
                name: "IX_SeoParameter_CreatedById",
                table: "SeoParameter",
                newName: "IX_SeoParameter_CreatedByID");

            migrationBuilder.RenameColumn(
                name: "UpdatedById",
                table: "SalePoint",
                newName: "UpdatedByID");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "SalePoint",
                newName: "CreatedByID");

            migrationBuilder.RenameIndex(
                name: "IX_SalePoint_UpdatedById",
                table: "SalePoint",
                newName: "IX_SalePoint_UpdatedByID");

            migrationBuilder.RenameIndex(
                name: "IX_SalePoint_CreatedById",
                table: "SalePoint",
                newName: "IX_SalePoint_CreatedByID");

            migrationBuilder.RenameColumn(
                name: "UpdatedById",
                table: "PartProducer",
                newName: "UpdatedByID");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "PartProducer",
                newName: "CreatedByID");

            migrationBuilder.RenameIndex(
                name: "IX_PartProducer_UpdatedById",
                table: "PartProducer",
                newName: "IX_PartProducer_UpdatedByID");

            migrationBuilder.RenameIndex(
                name: "IX_PartProducer_CreatedById",
                table: "PartProducer",
                newName: "IX_PartProducer_CreatedByID");

            migrationBuilder.RenameColumn(
                name: "UpdatedById",
                table: "Contact",
                newName: "UpdatedByID");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "Contact",
                newName: "CreatedByID");

            migrationBuilder.RenameIndex(
                name: "IX_Contact_UpdatedById",
                table: "Contact",
                newName: "IX_Contact_UpdatedByID");

            migrationBuilder.RenameIndex(
                name: "IX_Contact_CreatedById",
                table: "Contact",
                newName: "IX_Contact_CreatedByID");

            migrationBuilder.RenameColumn(
                name: "UpdatedById",
                table: "Address",
                newName: "UpdatedByID");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "Address",
                newName: "CreatedByID");

            migrationBuilder.RenameIndex(
                name: "IX_Address_UpdatedById",
                table: "Address",
                newName: "IX_Address_UpdatedByID");

            migrationBuilder.RenameIndex(
                name: "IX_Address_CreatedById",
                table: "Address",
                newName: "IX_Address_CreatedByID");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_User_CreatedByID",
                table: "Address",
                column: "CreatedByID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Address_User_UpdatedByID",
                table: "Address",
                column: "UpdatedByID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_User_CreatedByID",
                table: "Contact",
                column: "CreatedByID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_User_UpdatedByID",
                table: "Contact",
                column: "UpdatedByID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PartProducer_User_CreatedByID",
                table: "PartProducer",
                column: "CreatedByID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PartProducer_User_UpdatedByID",
                table: "PartProducer",
                column: "UpdatedByID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalePoint_User_CreatedByID",
                table: "SalePoint",
                column: "CreatedByID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalePoint_User_UpdatedByID",
                table: "SalePoint",
                column: "UpdatedByID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SeoParameter_User_CreatedByID",
                table: "SeoParameter",
                column: "CreatedByID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SeoParameter_User_UpdatedByID",
                table: "SeoParameter",
                column: "UpdatedByID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierOfferFile_User_UserID",
                table: "SupplierOfferFile",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierWarehouse_User_CreatedByID",
                table: "SupplierWarehouse",
                column: "CreatedByID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierWarehouse_User_UpdatedByID",
                table: "SupplierWarehouse",
                column: "UpdatedByID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
