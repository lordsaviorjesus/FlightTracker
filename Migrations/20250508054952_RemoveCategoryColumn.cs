using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace flight_tracker.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCategoryColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "category",
                table: "FlightRecords_EF");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "category",
                table: "FlightRecords_EF",
                type: "integer",
                nullable: true);
        }
    }
}
