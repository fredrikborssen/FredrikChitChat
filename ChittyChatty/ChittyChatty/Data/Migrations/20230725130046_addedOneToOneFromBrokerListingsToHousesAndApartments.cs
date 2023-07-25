using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChittyChatty.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedOneToOneFromBrokerListingsToHousesAndApartments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_brokerListings_BuildId",
                table: "brokerListings",
                column: "BuildId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_brokerListings_Apartments_BuildId",
                table: "brokerListings",
                column: "BuildId",
                principalTable: "Apartments",
                principalColumn: "BuildingId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_brokerListings_Houses_BuildId",
                table: "brokerListings",
                column: "BuildId",
                principalTable: "Houses",
                principalColumn: "BuildingId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_brokerListings_Apartments_BuildId",
                table: "brokerListings");

            migrationBuilder.DropForeignKey(
                name: "FK_brokerListings_Houses_BuildId",
                table: "brokerListings");

            migrationBuilder.DropIndex(
                name: "IX_brokerListings_BuildId",
                table: "brokerListings");
        }
    }
}
