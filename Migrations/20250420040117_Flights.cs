using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace flight_tracker.Migrations
{
    /// <inheritdoc />
    public partial class Flights : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlightRecords_EF",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    icao24 = table.Column<string>(type: "text", nullable: true),
                    callsign = table.Column<string>(type: "text", nullable: true),
                    origincountry = table.Column<string>(type: "text", nullable: true),
                    timeposition = table.Column<long>(type: "bigint", nullable: true),
                    lastcontact = table.Column<long>(type: "bigint", nullable: true),
                    longitude = table.Column<double>(type: "double precision", nullable: true),
                    latitude = table.Column<double>(type: "double precision", nullable: true),
                    baroaltitude = table.Column<double>(type: "double precision", nullable: true),
                    onground = table.Column<bool>(type: "boolean", nullable: true),
                    velocity = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightRecords_EF", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlightRecords_EF");
        }
    }
}
