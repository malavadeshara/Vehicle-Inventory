using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vehicle_Inventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestRideBookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SlotIndex = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DecisionAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestRideBookings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestRideBookings_Status",
                table: "TestRideBookings",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TestRideBookings_UserId",
                table: "TestRideBookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TestRideBookings_VehicleId",
                table: "TestRideBookings",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_TestRideBookings_VehicleId_BookingDate_SlotIndex",
                table: "TestRideBookings",
                columns: new[] { "VehicleId", "BookingDate", "SlotIndex" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestRideBookings");
        }
    }
}
