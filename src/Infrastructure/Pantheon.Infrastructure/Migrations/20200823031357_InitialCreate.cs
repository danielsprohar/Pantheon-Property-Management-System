﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pantheon.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    EmployeeId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FirstName = table.Column<string>(maxLength: 32, nullable: false),
                    MiddleName = table.Column<string>(maxLength: 32, nullable: true),
                    LastName = table.Column<string>(maxLength: 32, nullable: false),
                    Gender = table.Column<string>(maxLength: 1, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 16, nullable: false),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    DriverLicense_Number = table.Column<string>(maxLength: 32, nullable: true),
                    DriverLicense_State = table.Column<string>(maxLength: 32, nullable: true),
                    DriverLicense_PhotoUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Description = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParkingSpaceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    SpaceType = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingSpaceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Method = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RentalAgreementTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    AgreementType = table.Column<string>(maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalAgreementTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerVehicles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Make = table.Column<string>(maxLength: 32, nullable: false),
                    Model = table.Column<string>(maxLength: 64, nullable: false),
                    Color = table.Column<string>(maxLength: 32, nullable: false),
                    LicensePlateState = table.Column<string>(nullable: true),
                    LicensePlateNumber = table.Column<string>(nullable: true),
                    CustomerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerVehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerVehicles_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParkingSpaces",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    EmployeeId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Name = table.Column<string>(maxLength: 4, nullable: false),
                    Description = table.Column<string>(maxLength: 32, nullable: true),
                    RecurringRate = table.Column<decimal>(type: "DECIMAL(10, 2)", nullable: false),
                    IsAvailable = table.Column<bool>(nullable: false, defaultValue: true),
                    Amps = table.Column<int>(nullable: true),
                    Comments = table.Column<string>(maxLength: 2048, nullable: true),
                    ParkingSpaceTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingSpaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParkingSpaces_ParkingSpaceTypes_ParkingSpaceTypeId",
                        column: x => x.ParkingSpaceTypeId,
                        principalTable: "ParkingSpaceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    EmployeeId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Amount = table.Column<decimal>(type: "DECIMAL(10, 2)", nullable: false),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    IsRefund = table.Column<bool>(nullable: false, defaultValue: false),
                    PaymentMethodId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RentalAgreements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    EmployeeId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    RecurringDueDate = table.Column<int>(nullable: false),
                    TerminatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RentalAgreementTypeId = table.Column<int>(nullable: false),
                    ParkingSpaceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalAgreements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentalAgreements_ParkingSpaces_ParkingSpaceId",
                        column: x => x.ParkingSpaceId,
                        principalTable: "ParkingSpaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentalAgreements_RentalAgreementTypes_RentalAgreementTypeId",
                        column: x => x.RentalAgreementTypeId,
                        principalTable: "RentalAgreementTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerRentalAgreements",
                columns: table => new
                {
                    CustomerId = table.Column<int>(nullable: false),
                    RentalAgreementId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerRentalAgreements", x => new { x.CustomerId, x.RentalAgreementId });
                    table.ForeignKey(
                        name: "FK_CustomerRentalAgreements_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerRentalAgreements_RentalAgreements_RentalAgreementId",
                        column: x => x.RentalAgreementId,
                        principalTable: "RentalAgreements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    EmployeeId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DueDate = table.Column<DateTime>(type: "DATE", nullable: false),
                    BillingPeriodStart = table.Column<DateTime>(type: "DATE", nullable: false),
                    BillingPeriodEnd = table.Column<DateTime>(type: "DATE", nullable: false),
                    Comments = table.Column<string>(maxLength: 2048, nullable: true),
                    RentalAgreementId = table.Column<int>(nullable: false),
                    InvoiceStatusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_InvoiceStatuses_InvoiceStatusId",
                        column: x => x.InvoiceStatusId,
                        principalTable: "InvoiceStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoices_RentalAgreements_RentalAgreementId",
                        column: x => x.RentalAgreementId,
                        principalTable: "RentalAgreements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceLine",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(nullable: false),
                    ParkingSpaceId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 256, nullable: false),
                    Price = table.Column<decimal>(type: "DECIMAL(10, 2)", nullable: false),
                    Total = table.Column<decimal>(type: "DECIMAL(10, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceLine", x => new { x.InvoiceId, x.ParkingSpaceId });
                    table.ForeignKey(
                        name: "FK_InvoiceLine_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceLine_ParkingSpaces_ParkingSpaceId",
                        column: x => x.ParkingSpaceId,
                        principalTable: "ParkingSpaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoicePayments",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(nullable: false),
                    PaymentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoicePayments", x => new { x.InvoiceId, x.PaymentId });
                    table.ForeignKey(
                        name: "FK_InvoicePayments_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoicePayments_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Email", "EmployeeId", "FirstName", "Gender", "IsActive", "LastName", "MiddleName", "ModifiedBy", "NormalizedEmail", "PhoneNumber" },
                values: new object[] { 1, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "Alice", "F", null, "Smite", null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), null, "555-555-5555" });

            migrationBuilder.InsertData(
                table: "InvoiceStatuses",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { 1, "awaiting payment" },
                    { 2, "bad debt" },
                    { 3, "draft" },
                    { 4, "paid" },
                    { 5, "past due" },
                    { 6, "partial" },
                    { 7, "void" }
                });

            migrationBuilder.InsertData(
                table: "ParkingSpaceTypes",
                columns: new[] { "Id", "SpaceType" },
                values: new object[,]
                {
                    { 3, "house" },
                    { 2, "mobile_home_space" },
                    { 1, "rv_space" }
                });

            migrationBuilder.InsertData(
                table: "PaymentMethods",
                columns: new[] { "Id", "Method" },
                values: new object[,]
                {
                    { 1, "cash" },
                    { 2, "check" },
                    { 3, "credit" },
                    { 4, "debit" },
                    { 5, "money_order" }
                });

            migrationBuilder.InsertData(
                table: "RentalAgreementTypes",
                columns: new[] { "Id", "AgreementType" },
                values: new object[,]
                {
                    { 1, "daily" },
                    { 2, "monthly" },
                    { 3, "weekly" }
                });

            migrationBuilder.InsertData(
                table: "CustomerVehicles",
                columns: new[] { "Id", "Color", "CustomerId", "LicensePlateNumber", "LicensePlateState", "Make", "Model", "Year" },
                values: new object[] { 1, "blue", 1, "1234-ASD", "Texas", "Ford", "Mustang GT", 2007 });

            migrationBuilder.InsertData(
                table: "ParkingSpaces",
                columns: new[] { "Id", "Amps", "Comments", "Description", "EmployeeId", "ModifiedBy", "Name", "ParkingSpaceTypeId", "RecurringRate" },
                values: new object[,]
                {
                    { 30, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "30", 1, 400m },
                    { 29, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "29", 1, 400m },
                    { 28, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "28", 1, 400m },
                    { 27, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "27", 1, 400m },
                    { 26, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "26", 1, 400m },
                    { 25, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "25", 1, 400m },
                    { 24, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "24", 1, 400m },
                    { 23, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "23", 1, 400m },
                    { 22, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "22", 1, 400m },
                    { 21, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "21", 1, 400m },
                    { 20, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "20", 1, 400m },
                    { 19, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "19", 1, 400m },
                    { 18, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "18", 1, 400m },
                    { 17, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "17", 1, 400m },
                    { 31, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "31", 1, 400m },
                    { 16, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "16", 1, 400m },
                    { 14, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "14", 1, 400m },
                    { 13, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "13", 1, 400m },
                    { 12, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "12", 1, 400m },
                    { 11, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "11", 1, 400m },
                    { 10, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "10", 1, 400m },
                    { 9, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "9", 1, 400m },
                    { 8, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "8", 1, 400m },
                    { 7, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "7", 1, 400m },
                    { 6, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "6", 1, 400m },
                    { 5, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "5", 1, 400m },
                    { 4, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "4", 1, 400m },
                    { 3, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "3", 1, 400m },
                    { 2, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "2", 1, 400m },
                    { 1, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "1", 1, 400m },
                    { 15, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "15", 1, 400m },
                    { 32, 30, null, null, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), "32", 1, 400m }
                });

            migrationBuilder.InsertData(
                table: "RentalAgreements",
                columns: new[] { "Id", "EmployeeId", "ModifiedBy", "ParkingSpaceId", "RecurringDueDate", "RentalAgreementTypeId", "TerminatedOn" },
                values: new object[] { 1, new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), 1, 1, 2, null });

            migrationBuilder.InsertData(
                table: "CustomerRentalAgreements",
                columns: new[] { "CustomerId", "RentalAgreementId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "Id", "BillingPeriodEnd", "BillingPeriodStart", "Comments", "DueDate", "EmployeeId", "InvoiceStatusId", "ModifiedBy", "RentalAgreementId" },
                values: new object[] { 1, new DateTime(2020, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2020, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5b628641-23b1-4aa3-99e0-a07bb3e5c4db"), 1, new Guid("00000000-0000-0000-0000-000000000000"), 1 });

            migrationBuilder.InsertData(
                table: "InvoiceLine",
                columns: new[] { "InvoiceId", "ParkingSpaceId", "Description", "Price", "Quantity", "Total" },
                values: new object[] { 1, 1, "Space #1; monthly rate of $400.00; electricity and water are included.", 400m, 1, 400m });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRentalAgreements_RentalAgreementId",
                table: "CustomerRentalAgreements",
                column: "RentalAgreementId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_FirstName_LastName",
                table: "Customers",
                columns: new[] { "FirstName", "LastName" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerVehicles_CustomerId",
                table: "CustomerVehicles",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLine_ParkingSpaceId",
                table: "InvoiceLine",
                column: "ParkingSpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoicePayments_PaymentId",
                table: "InvoicePayments",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_InvoiceStatusId",
                table: "Invoices",
                column: "InvoiceStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_RentalAgreementId",
                table: "Invoices",
                column: "RentalAgreementId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSpaces_Name",
                table: "ParkingSpaces",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSpaces_ParkingSpaceTypeId",
                table: "ParkingSpaces",
                column: "ParkingSpaceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CustomerId",
                table: "Payments",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentMethodId",
                table: "Payments",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalAgreements_ParkingSpaceId",
                table: "RentalAgreements",
                column: "ParkingSpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalAgreements_RentalAgreementTypeId",
                table: "RentalAgreements",
                column: "RentalAgreementTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerRentalAgreements");

            migrationBuilder.DropTable(
                name: "CustomerVehicles");

            migrationBuilder.DropTable(
                name: "InvoiceLine");

            migrationBuilder.DropTable(
                name: "InvoicePayments");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "InvoiceStatuses");

            migrationBuilder.DropTable(
                name: "RentalAgreements");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "ParkingSpaces");

            migrationBuilder.DropTable(
                name: "RentalAgreementTypes");

            migrationBuilder.DropTable(
                name: "ParkingSpaceTypes");
        }
    }
}
