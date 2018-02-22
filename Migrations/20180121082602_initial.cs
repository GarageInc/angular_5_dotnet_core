using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace test.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModelFile",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelFile", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Login = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    RoleId = table.Column<int>(nullable: true),
                    UpdatedAt = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<int>(nullable: false),
                    CreatedByID = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Locality = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Longitude = table.Column<double>(nullable: false),
                    UpdatedAt = table.Column<int>(nullable: false),
                    UpdatedByID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Address_User_CreatedByID",
                        column: x => x.CreatedByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Address_User_UpdatedByID",
                        column: x => x.UpdatedByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SeoParameter",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<int>(nullable: false),
                    CreatedByID = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<int>(nullable: false),
                    UpdatedByID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeoParameter", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SeoParameter_User_CreatedByID",
                        column: x => x.CreatedByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SeoParameter_User_UpdatedByID",
                        column: x => x.UpdatedByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PartProducer",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<int>(nullable: false),
                    CreatedByID = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LogoID = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SeoParameterId = table.Column<int>(nullable: true),
                    Site = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<int>(nullable: false),
                    UpdatedByID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartProducer", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PartProducer_User_CreatedByID",
                        column: x => x.CreatedByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartProducer_ModelFile_LogoID",
                        column: x => x.LogoID,
                        principalTable: "ModelFile",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartProducer_SeoParameter_SeoParameterId",
                        column: x => x.SeoParameterId,
                        principalTable: "SeoParameter",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartProducer_User_UpdatedByID",
                        column: x => x.UpdatedByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PartsSupplier",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    INN = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LogoID = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SearchName = table.Column<string>(nullable: true),
                    SeoParameterId = table.Column<int>(nullable: true),
                    Site = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartsSupplier", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PartsSupplier_ModelFile_LogoID",
                        column: x => x.LogoID,
                        principalTable: "ModelFile",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartsSupplier_SeoParameter_SeoParameterId",
                        column: x => x.SeoParameterId,
                        principalTable: "SeoParameter",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProducerCatalogItem",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<int>(nullable: false),
                    EnName = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ProducerCode = table.Column<string>(nullable: true),
                    ProducerCodeTrimmed = table.Column<string>(nullable: false),
                    ProducerId = table.Column<int>(nullable: false),
                    RuName = table.Column<string>(nullable: true),
                    SeoParameterId = table.Column<int>(nullable: true),
                    UpdatedAt = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProducerCatalogItem", x => x.ID);
                    table.UniqueConstraint("AK_ProducerCatalogItem_ProducerId_ProducerCodeTrimmed", x => new { x.ProducerId, x.ProducerCodeTrimmed });
                    table.ForeignKey(
                        name: "FK_ProducerCatalogItem_PartProducer_ProducerId",
                        column: x => x.ProducerId,
                        principalTable: "PartProducer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProducerCatalogItem_SeoParameter_SeoParameterId",
                        column: x => x.SeoParameterId,
                        principalTable: "SeoParameter",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProducerToProducer",
                columns: table => new
                {
                    FromId = table.Column<int>(nullable: false),
                    ToId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProducerToProducer", x => new { x.FromId, x.ToId });
                    table.ForeignKey(
                        name: "FK_ProducerToProducer_PartProducer_FromId",
                        column: x => x.FromId,
                        principalTable: "PartProducer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProducerToProducer_PartProducer_ToId",
                        column: x => x.ToId,
                        principalTable: "PartProducer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalePoint",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AddressID = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<int>(nullable: false),
                    CreatedByID = table.Column<int>(nullable: true),
                    DeliveryMethod = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PartsSupplierID = table.Column<int>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<int>(nullable: false),
                    UpdatedByID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalePoint", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SalePoint_Address_AddressID",
                        column: x => x.AddressID,
                        principalTable: "Address",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalePoint_User_CreatedByID",
                        column: x => x.CreatedByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalePoint_PartsSupplier_PartsSupplierID",
                        column: x => x.PartsSupplierID,
                        principalTable: "PartsSupplier",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalePoint_User_UpdatedByID",
                        column: x => x.UpdatedByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupplierPriceItem",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Count = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsAvailable = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastUploadedUpdated = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PartsSupplierId = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    PriceEu = table.Column<decimal>(nullable: false),
                    PriceUsd = table.Column<decimal>(nullable: false),
                    ProducerCode = table.Column<string>(nullable: true),
                    ProducerCodeTrimmed = table.Column<string>(nullable: false),
                    ProducerId = table.Column<int>(nullable: true),
                    ProducerName = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UpdatedAt = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierPriceItem", x => x.ID);
                    table.UniqueConstraint("AK_SupplierPriceItem_PartsSupplierId_ProducerCodeTrimmed", x => new { x.PartsSupplierId, x.ProducerCodeTrimmed });
                    table.ForeignKey(
                        name: "FK_SupplierPriceItem_PartsSupplier_PartsSupplierId",
                        column: x => x.PartsSupplierId,
                        principalTable: "PartsSupplier",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplierPriceItem_PartProducer_ProducerId",
                        column: x => x.ProducerId,
                        principalTable: "PartProducer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupplierWarehouse",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AddressID = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<int>(nullable: false),
                    CreatedByID = table.Column<int>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    IdForProducer = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    SupplierId = table.Column<int>(nullable: false),
                    UpdatedAt = table.Column<int>(nullable: false),
                    UpdatedByID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierWarehouse", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SupplierWarehouse_Address_AddressID",
                        column: x => x.AddressID,
                        principalTable: "Address",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplierWarehouse_User_CreatedByID",
                        column: x => x.CreatedByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplierWarehouse_PartsSupplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "PartsSupplier",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplierWarehouse_User_UpdatedByID",
                        column: x => x.UpdatedByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TimeWork",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SalePointID = table.Column<int>(nullable: true),
                    UpdatedAt = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeWork", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TimeWork_SalePoint_SalePointID",
                        column: x => x.SalePointID,
                        principalTable: "SalePoint",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AddressID = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<int>(nullable: false),
                    CreatedByID = table.Column<int>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<int>(nullable: false),
                    UpdatedByID = table.Column<int>(nullable: true),
                    PartsSupplierID = table.Column<int>(nullable: true),
                    SupplierWarehouseID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Contact_Address_AddressID",
                        column: x => x.AddressID,
                        principalTable: "Address",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contact_User_CreatedByID",
                        column: x => x.CreatedByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contact_User_UpdatedByID",
                        column: x => x.UpdatedByID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contact_PartsSupplier_PartsSupplierID",
                        column: x => x.PartsSupplierID,
                        principalTable: "PartsSupplier",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contact_SupplierWarehouse_SupplierWarehouseID",
                        column: x => x.SupplierWarehouseID,
                        principalTable: "SupplierWarehouse",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_CreatedByID",
                table: "Address",
                column: "CreatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_Address_UpdatedByID",
                table: "Address",
                column: "UpdatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_AddressID",
                table: "Contact",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_CreatedByID",
                table: "Contact",
                column: "CreatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_UpdatedByID",
                table: "Contact",
                column: "UpdatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_PartsSupplierID",
                table: "Contact",
                column: "PartsSupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_SupplierWarehouseID",
                table: "Contact",
                column: "SupplierWarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_PartProducer_CreatedByID",
                table: "PartProducer",
                column: "CreatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_PartProducer_LogoID",
                table: "PartProducer",
                column: "LogoID");

            migrationBuilder.CreateIndex(
                name: "IX_PartProducer_SeoParameterId",
                table: "PartProducer",
                column: "SeoParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_PartProducer_UpdatedByID",
                table: "PartProducer",
                column: "UpdatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_PartsSupplier_LogoID",
                table: "PartsSupplier",
                column: "LogoID");

            migrationBuilder.CreateIndex(
                name: "IX_PartsSupplier_SeoParameterId",
                table: "PartsSupplier",
                column: "SeoParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_ProducerCatalogItem_ProducerCodeTrimmed",
                table: "ProducerCatalogItem",
                column: "ProducerCodeTrimmed");

            migrationBuilder.CreateIndex(
                name: "IX_ProducerCatalogItem_SeoParameterId",
                table: "ProducerCatalogItem",
                column: "SeoParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_ProducerToProducer_ToId",
                table: "ProducerToProducer",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_SalePoint_AddressID",
                table: "SalePoint",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_SalePoint_CreatedByID",
                table: "SalePoint",
                column: "CreatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_SalePoint_PartsSupplierID",
                table: "SalePoint",
                column: "PartsSupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_SalePoint_UpdatedByID",
                table: "SalePoint",
                column: "UpdatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_SeoParameter_CreatedByID",
                table: "SeoParameter",
                column: "CreatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_SeoParameter_UpdatedByID",
                table: "SeoParameter",
                column: "UpdatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierPriceItem_ProducerCodeTrimmed",
                table: "SupplierPriceItem",
                column: "ProducerCodeTrimmed");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierPriceItem_ProducerId",
                table: "SupplierPriceItem",
                column: "ProducerId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierWarehouse_AddressID",
                table: "SupplierWarehouse",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierWarehouse_CreatedByID",
                table: "SupplierWarehouse",
                column: "CreatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierWarehouse_SupplierId",
                table: "SupplierWarehouse",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierWarehouse_UpdatedByID",
                table: "SupplierWarehouse",
                column: "UpdatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_TimeWork_SalePointID",
                table: "TimeWork",
                column: "SalePointID");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "ProducerCatalogItem");

            migrationBuilder.DropTable(
                name: "ProducerToProducer");

            migrationBuilder.DropTable(
                name: "SupplierPriceItem");

            migrationBuilder.DropTable(
                name: "TimeWork");

            migrationBuilder.DropTable(
                name: "SupplierWarehouse");

            migrationBuilder.DropTable(
                name: "PartProducer");

            migrationBuilder.DropTable(
                name: "SalePoint");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "PartsSupplier");

            migrationBuilder.DropTable(
                name: "ModelFile");

            migrationBuilder.DropTable(
                name: "SeoParameter");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
