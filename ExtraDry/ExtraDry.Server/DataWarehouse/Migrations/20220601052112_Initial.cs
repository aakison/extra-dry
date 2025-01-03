using Microsoft.EntityFrameworkCore.Migrations;

namespace ExtraDry.Server.DataWarehouse.Migrations;

public partial class Initial : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "__EDDataFactorySync",
            columns: table => new {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Table = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Schema = table.Column<string>(type: "nvarchar(max)", nullable: false),
                SyncTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table => {
                table.PrimaryKey("PK___EDDataFactorySync", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "__EDDataFactorySync");
    }
}
