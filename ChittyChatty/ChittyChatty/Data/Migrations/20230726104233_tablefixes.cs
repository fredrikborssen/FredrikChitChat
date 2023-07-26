using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChittyChatty.Data.Migrations
{
    /// <inheritdoc />
    public partial class tablefixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrokerListings");

            migrationBuilder.CreateTable(
                name: "BrokerListingApartments",
                columns: table => new
                {
                    BrokerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrokerListingApartments", x => new { x.BrokerId, x.BuildingId });
                    table.ForeignKey(
                        name: "FK_BrokerListingApartments_Apartments_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Apartments",
                        principalColumn: "BuildingId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BrokerListingApartments_Brokers_BrokerId",
                        column: x => x.BrokerId,
                        principalTable: "Brokers",
                        principalColumn: "BrokerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BrokerListingHouses",
                columns: table => new
                {
                    BrokerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrokerListingHouses", x => new { x.BrokerId, x.BuildingId });
                    table.ForeignKey(
                        name: "FK_BrokerListingHouses_Brokers_BrokerId",
                        column: x => x.BrokerId,
                        principalTable: "Brokers",
                        principalColumn: "BrokerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrokerListingHouses_Houses_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Houses",
                        principalColumn: "BuildingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrokerListingApartments_BuildingId",
                table: "BrokerListingApartments",
                column: "BuildingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BrokerListingHouses_BuildingId",
                table: "BrokerListingHouses",
                column: "BuildingId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrokerListingApartments");

            migrationBuilder.DropTable(
                name: "BrokerListingHouses");

            migrationBuilder.CreateTable(
                name: "BrokerListings",
                columns: table => new
                {
                    BrokerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrokerListings", x => new { x.BrokerId, x.BuildingId });
                    table.ForeignKey(
                        name: "FK_BrokerListings_Apartments_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Apartments",
                        principalColumn: "BuildingId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BrokerListings_Brokers_BrokerId",
                        column: x => x.BrokerId,
                        principalTable: "Brokers",
                        principalColumn: "BrokerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrokerListings_Houses_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Houses",
                        principalColumn: "BuildingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrokerListings_BuildingId",
                table: "BrokerListings",
                column: "BuildingId",
                unique: true);
        }
    }
}
