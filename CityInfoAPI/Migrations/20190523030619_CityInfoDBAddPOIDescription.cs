using Microsoft.EntityFrameworkCore.Migrations;

namespace CityInfoAPI.Migrations
{
    public partial class CityInfoDBAddPOIDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "poinstOfInterest",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "poinstOfInterest");
        }
    }
}
