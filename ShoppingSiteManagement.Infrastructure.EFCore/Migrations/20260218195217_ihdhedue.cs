using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingSiteManagement.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class ihdhedue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeExpiration",
                table: "Accounts");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Accounts");

            migrationBuilder.AddColumn<DateTime>(
                name: "CodeExpiration",
                table: "Accounts",
                type: "datetime2",
                nullable: true);
        }
    }
}
