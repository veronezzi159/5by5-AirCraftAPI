using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _5by5_AirCraftAPI.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AirCraft",
                columns: table => new
                {
                    Rab = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    DTRegistry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DTLastFlight = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CnpjCompany = table.Column<string>(type: "nvarchar(19)", maxLength: 19, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirCraft", x => x.Rab);
                });

            migrationBuilder.CreateTable(
                name: "Removed",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rab = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    DTRegistry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DTLastFlight = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CnpjCompany = table.Column<string>(type: "nvarchar(19)", maxLength: 19, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Removed", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirCraft");

            migrationBuilder.DropTable(
                name: "Removed");
        }
    }
}
