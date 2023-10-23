using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Data.Migrations
{
    /// <inheritdoc />
    public partial class Hierarchy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<HierarchyId>(
                name: "AncestorList",
                table: "Regions",
                type: "hierarchyid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Regions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomFields",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValueSql: "'{}'",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
            
            //Update ParentId
            migrationBuilder.Sql("Update Child Set " +
               "Child.ParentId = Parent.Id " +
               "From Regions child " +
               "LEFT JOIN RegionRegion ON DescendantsId = child.Id " +
               "LEFT JOIN Regions Parent on AncestorsId = Parent.Id " +
               "Where Parent.Level = child.Level - 1 " +
               "AND child.ParentId IS NULL");

            // Update AncestorList
            migrationBuilder.Sql("with c2 as ( " +
                "select base.*, cast(base.Id as varchar(max)) as Lineage " +
                "from Regions base " +
                "where base.Level = 0 " +
                "union all " +
                "select " +
                "child.*, " +
                "c2.Lineage + '/' + cast(child.Id as varchar(max)) as Lineage " +
                "from " +
                "c2 join Regions child on c2.Id = child.ParentId ) " +
                "Update Regions " +
                "Set AncestorList = '/' + c2.Lineage + '/' " +
                "from c2 " +
                "Where c2.Uuid = Regions.Uuid " +
                "AND Regions.AncestorList IS NULL");


            migrationBuilder.DropTable(
               name: "RegionRegion");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_ParentId",
                table: "Regions",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Regions_Regions_ParentId",
                table: "Regions",
                column: "ParentId",
                principalTable: "Regions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Regions_Regions_ParentId",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Regions_ParentId",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "AncestorList",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Regions");

            migrationBuilder.AlterColumn<string>(
                name: "CustomFields",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValueSql: "'{}'");

            migrationBuilder.CreateTable(
                name: "RegionRegion",
                columns: table => new
                {
                    AncestorsId = table.Column<int>(type: "int", nullable: false),
                    DescendantsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionRegion", x => new { x.AncestorsId, x.DescendantsId });
                    table.ForeignKey(
                        name: "FK_RegionRegion_Regions_AncestorsId",
                        column: x => x.AncestorsId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegionRegion_Regions_DescendantsId",
                        column: x => x.DescendantsId,
                        principalTable: "Regions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegionRegion_DescendantsId",
                table: "RegionRegion",
                column: "DescendantsId");
        }
    }
}
