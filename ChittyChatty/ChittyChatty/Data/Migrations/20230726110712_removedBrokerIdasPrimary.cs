using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChittyChatty.Data.Migrations
{
    /// <inheritdoc />
    public partial class removedBrokerIdasPrimary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BrokerListingHouses",
                table: "BrokerListingHouses");

            migrationBuilder.DropIndex(
                name: "IX_BrokerListingHouses_BuildingId",
                table: "BrokerListingHouses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BrokerListingApartments",
                table: "BrokerListingApartments");

            migrationBuilder.DropIndex(
                name: "IX_BrokerListingApartments_BuildingId",
                table: "BrokerListingApartments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BrokerListingHouses",
                table: "BrokerListingHouses",
                column: "BuildingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BrokerListingApartments",
                table: "BrokerListingApartments",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_BrokerListingHouses_BrokerId",
                table: "BrokerListingHouses",
                column: "BrokerId");

            migrationBuilder.CreateIndex(
                name: "IX_BrokerListingApartments_BrokerId",
                table: "BrokerListingApartments",
                column: "BrokerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BrokerListingHouses",
                table: "BrokerListingHouses");

            migrationBuilder.DropIndex(
                name: "IX_BrokerListingHouses_BrokerId",
                table: "BrokerListingHouses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BrokerListingApartments",
                table: "BrokerListingApartments");

            migrationBuilder.DropIndex(
                name: "IX_BrokerListingApartments_BrokerId",
                table: "BrokerListingApartments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BrokerListingHouses",
                table: "BrokerListingHouses",
                columns: new[] { "BrokerId", "BuildingId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_BrokerListingApartments",
                table: "BrokerListingApartments",
                columns: new[] { "BrokerId", "BuildingId" });

            migrationBuilder.CreateIndex(
                name: "IX_BrokerListingHouses_BuildingId",
                table: "BrokerListingHouses",
                column: "BuildingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BrokerListingApartments_BuildingId",
                table: "BrokerListingApartments",
                column: "BuildingId",
                unique: true);
        }
    }
}
