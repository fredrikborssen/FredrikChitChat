using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChittyChatty.Data.Migrations
{
    /// <inheritdoc />
    public partial class cascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrokerListingApartments_Apartments_BuildingId",
                table: "BrokerListingApartments");

            migrationBuilder.DropForeignKey(
                name: "FK_BrokerListingHouses_Houses_BuildingId",
                table: "BrokerListingHouses");

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerListingApartments_Apartments_BuildingId",
                table: "BrokerListingApartments",
                column: "BuildingId",
                principalTable: "Apartments",
                principalColumn: "BuildingId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerListingHouses_Houses_BuildingId",
                table: "BrokerListingHouses",
                column: "BuildingId",
                principalTable: "Houses",
                principalColumn: "BuildingId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrokerListingApartments_Apartments_BuildingId",
                table: "BrokerListingApartments");

            migrationBuilder.DropForeignKey(
                name: "FK_BrokerListingHouses_Houses_BuildingId",
                table: "BrokerListingHouses");

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerListingApartments_Apartments_BuildingId",
                table: "BrokerListingApartments",
                column: "BuildingId",
                principalTable: "Apartments",
                principalColumn: "BuildingId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerListingHouses_Houses_BuildingId",
                table: "BrokerListingHouses",
                column: "BuildingId",
                principalTable: "Houses",
                principalColumn: "BuildingId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
