using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microservices.Precipitation.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Preceipitation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AmountInches = table.Column<decimal>(type: "numeric", nullable: false),
                    WeatherType = table.Column<string>(type: "text", nullable: false),
                    Zipcode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preceipitation", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Preceipitation");
        }
    }
}
