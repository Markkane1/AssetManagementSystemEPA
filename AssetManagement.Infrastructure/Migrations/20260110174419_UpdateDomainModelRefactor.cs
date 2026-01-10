using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDomainModelRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignmentStatus",
                table: "AssetItems");

            migrationBuilder.AddColumn<int>(
                name: "AssignedByEmployeeId",
                table: "Assignments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignmentStatus",
                table: "Assignments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReturnNotes",
                table: "Assignments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReturnedByEmployeeId",
                table: "Assignments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssetCode",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Assets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Assets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Assets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Specification",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UntrackedQuantity",
                table: "Assets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Assets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WarrantyEndDate",
                table: "Assets",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedByEmployeeId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "AssignmentStatus",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "ReturnNotes",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "ReturnedByEmployeeId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "AssetCode",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "Specification",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "UntrackedQuantity",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "WarrantyEndDate",
                table: "Assets");

            migrationBuilder.AddColumn<string>(
                name: "AssignmentStatus",
                table: "AssetItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
