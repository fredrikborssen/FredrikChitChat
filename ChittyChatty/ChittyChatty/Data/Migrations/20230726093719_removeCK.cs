using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChittyChatty.Data.Migrations
{
    /// <inheritdoc />
    public partial class removeCK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_BuildId_Houses_Apartments",
                table: "BrokerListings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_BuildId_Houses_Apartments",
                table: "BrokerListings",
                sql: "[BuildingId] IS NOT NULL OR [BuildingId] IS NOT NULL");
        }
    }
}
