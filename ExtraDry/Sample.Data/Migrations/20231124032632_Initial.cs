using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Scope = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false),
                    ShaHash = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Layout = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VersionDateCreated = table.Column<DateTime>(name: "Version_DateCreated", type: "datetime2", nullable: false),
                    VersionUserCreated = table.Column<string>(name: "Version_UserCreated", type: "nvarchar(80)", maxLength: 80, nullable: false),
                    VersionDateModified = table.Column<DateTime>(name: "Version_DateModified", type: "datetime2", nullable: false),
                    VersionUserModified = table.Column<string>(name: "Version_UserModified", type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    TerminationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VersionDateCreated = table.Column<DateTime>(name: "Version_DateCreated", type: "datetime2", nullable: false),
                    VersionUserCreated = table.Column<string>(name: "Version_UserCreated", type: "nvarchar(80)", maxLength: 80, nullable: false),
                    VersionDateModified = table.Column<DateTime>(name: "Version_DateModified", type: "datetime2", nullable: false),
                    VersionUserModified = table.Column<string>(name: "Version_UserModified", type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Lineage = table.Column<HierarchyId>(type: "hierarchyid", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<int>(type: "int", nullable: false),
                    VersionDateCreated = table.Column<DateTime>(name: "Version_DateCreated", type: "datetime2", nullable: false),
                    VersionUserCreated = table.Column<string>(name: "Version_UserCreated", type: "nvarchar(80)", maxLength: 80, nullable: false),
                    VersionDateModified = table.Column<DateTime>(name: "Version_DateModified", type: "datetime2", nullable: false),
                    VersionUserModified = table.Column<string>(name: "Version_UserModified", type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Regions_Regions_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Regions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    VersionDateCreated = table.Column<DateTime>(name: "Version_DateCreated", type: "datetime2", nullable: false),
                    VersionUserCreated = table.Column<string>(name: "Version_UserCreated", type: "nvarchar(80)", maxLength: 80, nullable: false),
                    VersionDateModified = table.Column<DateTime>(name: "Version_DateModified", type: "datetime2", nullable: false),
                    VersionUserModified = table.Column<string>(name: "Version_UserModified", type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Schema = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    PrimarySectorId = table.Column<int>(type: "int", nullable: true),
                    Ownership = table.Column<int>(type: "int", nullable: false),
                    ContactPhone = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AnnualRevenue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SalesMargin = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IncorporationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BankingDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VersionDateCreated = table.Column<DateTime>(name: "Version_DateCreated", type: "datetime2", nullable: false),
                    VersionUserCreated = table.Column<string>(name: "Version_UserCreated", type: "nvarchar(80)", maxLength: 80, nullable: false),
                    VersionDateModified = table.Column<DateTime>(name: "Version_DateModified", type: "datetime2", nullable: false),
                    VersionUserModified = table.Column<string>(name: "Version_UserModified", type: "nvarchar(80)", maxLength: 80, nullable: false),
                    CustomFields = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValueSql: "'{}'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sectors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Group = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    VersionDateCreated = table.Column<DateTime>(name: "Version_DateCreated", type: "datetime2", nullable: false),
                    VersionUserCreated = table.Column<string>(name: "Version_UserCreated", type: "nvarchar(80)", maxLength: 80, nullable: false),
                    VersionDateModified = table.Column<DateTime>(name: "Version_DateModified", type: "datetime2", nullable: false),
                    VersionUserModified = table.Column<string>(name: "Version_UserModified", type: "nvarchar(80)", maxLength: 80, nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sectors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sectors_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_PrimarySectorId",
                table: "Companies",
                column: "PrimarySectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_ParentId",
                table: "Regions",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Uuid",
                table: "Regions",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sectors_CompanyId",
                table: "Sectors",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Sectors_PrimarySectorId",
                table: "Companies",
                column: "PrimarySectorId",
                principalTable: "Sectors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Sectors_PrimarySectorId",
                table: "Companies");

            migrationBuilder.DropTable(
                name: "Blobs");

            migrationBuilder.DropTable(
                name: "Contents");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "Sectors");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
