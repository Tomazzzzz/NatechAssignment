using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NatechAssignmentDataLayer.Migrations
{
    /// <inheritdoc />
    public partial class Updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Processed",
                table: "Headers");

            migrationBuilder.DropColumn(
                name: "TotalToProcess",
                table: "Headers");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Headers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "Headers");

            migrationBuilder.AddColumn<int>(
                name: "Processed",
                table: "Headers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalToProcess",
                table: "Headers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
