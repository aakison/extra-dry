using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Data.Migrations
{
    /// <inheritdoc />
    public partial class Templates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Templates");
        }
    }
}
