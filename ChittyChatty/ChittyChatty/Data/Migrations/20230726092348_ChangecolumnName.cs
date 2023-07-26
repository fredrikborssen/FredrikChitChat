using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChittyChatty.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangecolumnName : Migration
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

            migrationBuilder.RenameColumn(
                name: "BuildId",
                table: "BrokerListings",
                newName: "BuildingId");

            migrationBuilder.RenameIndex(
                name: "IX_BrokerListings_BuildId",
                table: "BrokerListings",
                newName: "IX_BrokerListings_BuildingId");

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerListings_Apartments_BuildingId",
                table: "BrokerListings",
                column: "BuildingId",
                principalTable: "Apartments",
                principalColumn: "BuildingId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerListings_Houses_BuildingId",
                table: "BrokerListings",
                column: "BuildingId",
                principalTable: "Houses",
                principalColumn: "BuildingId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrokerListings_Apartments_BuildingId",
                table: "BrokerListings");

            migrationBuilder.DropForeignKey(
                name: "FK_BrokerListings_Houses_BuildingId",
                table: "BrokerListings");

            migrationBuilder.RenameColumn(
                name: "BuildingId",
                table: "BrokerListings",
                newName: "BuildId");

            migrationBuilder.RenameIndex(
                name: "IX_BrokerListings_BuildingId",
                table: "BrokerListings",
                newName: "IX_BrokerListings_BuildId");

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
    }
}
