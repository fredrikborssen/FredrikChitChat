using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChittyChatty.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedBrokersInTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Publisher",
                table: "Houses",
                newName: "BrokerCompany");

            migrationBuilder.RenameColumn(
                name: "Publisher",
                table: "Apartments",
                newName: "BrokerCompany");

            migrationBuilder.AddColumn<int>(
                name: "BrokerId",
                table: "Houses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BrokerId",
                table: "Apartments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrokerId",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "BrokerId",
                table: "Apartments");

            migrationBuilder.RenameColumn(
                name: "BrokerCompany",
                table: "Houses",
                newName: "Publisher");

            migrationBuilder.RenameColumn(
                name: "BrokerCompany",
                table: "Apartments",
                newName: "Publisher");
        }
    }
}
