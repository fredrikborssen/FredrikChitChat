using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChittyChatty.Data.Migrations
{
    /// <inheritdoc />
    public partial class changedRequiredToFalse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrokerListings_Apartments_BuildId",
                table: "BrokerListings");

            migrationBuilder.DropForeignKey(
                name: "FK_BrokerListings_Houses_BuildId",
                table: "BrokerListings");

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerListings_Apartments_BuildId",
                table: "BrokerListings",
                column: "BuildId",
                principalTable: "Apartments",
                principalColumn: "BuildingId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerListings_Houses_BuildId",
                table: "BrokerListings",
                column: "BuildId",
                principalTable: "Houses",
                principalColumn: "BuildingId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrokerListings_Apartments_BuildId",
                table: "BrokerListings");

            migrationBuilder.DropForeignKey(
                name: "FK_BrokerListings_Houses_BuildId",
                table: "BrokerListings");

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerListings_Apartments_BuildId",
                table: "BrokerListings",
                column: "BuildId",
                principalTable: "Apartments",
                principalColumn: "BuildingId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerListings_Houses_BuildId",
                table: "BrokerListings",
                column: "BuildId",
                principalTable: "Houses",
                principalColumn: "BuildingId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
