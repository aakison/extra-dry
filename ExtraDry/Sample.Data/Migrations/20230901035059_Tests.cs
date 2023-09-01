using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Data.Migrations
{
    /// <inheritdoc />
    public partial class Tests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Regions",
                newName: "Slug");

            migrationBuilder.AddColumn<int>(
                name: "IsDeleted",
                table: "Regions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "Uuid",
                table: "Regions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "Uuid",
                table: "Regions");

            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "Regions",
                newName: "Code");
        }
    }
}
