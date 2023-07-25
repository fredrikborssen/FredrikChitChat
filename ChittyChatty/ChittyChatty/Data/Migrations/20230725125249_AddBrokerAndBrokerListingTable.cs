using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChittyChatty.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBrokerAndBrokerListingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Houses",
                newName: "BuildingId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Apartments",
                newName: "BuildingId");

            migrationBuilder.CreateTable(
                name: "Brokers",
                columns: table => new
                {
                    BrokerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrokerCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brokers", x => x.BrokerId);
                });

            migrationBuilder.CreateTable(
                name: "brokerListings",
                columns: table => new
                {
                    BrokerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BuildId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_brokerListings", x => new { x.BrokerId, x.BuildId });
                    table.ForeignKey(
                        name: "FK_brokerListings_Brokers_BrokerId",
                        column: x => x.BrokerId,
                        principalTable: "Brokers",
                        principalColumn: "BrokerId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "brokerListings");

            migrationBuilder.DropTable(
                name: "Brokers");

            migrationBuilder.RenameColumn(
                name: "BuildingId",
                table: "Houses",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "BuildingId",
                table: "Apartments",
                newName: "Id");
        }
    }
}
