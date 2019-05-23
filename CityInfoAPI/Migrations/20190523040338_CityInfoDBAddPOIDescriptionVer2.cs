using Microsoft.EntityFrameworkCore.Migrations;

namespace CityInfoAPI.Migrations
{
    public partial class CityInfoDBAddPOIDescriptionVer2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "description",
                table: "poinstOfInterest",
                newName: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "poinstOfInterest",
                newName: "description");
        }
    }
}
