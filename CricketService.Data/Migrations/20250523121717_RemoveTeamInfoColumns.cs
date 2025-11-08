using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CricketService.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTeamInfoColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "odi_records",
                table: "cricket_teams_info");

            migrationBuilder.DropColumn(
                name: "t20i_records",
                table: "cricket_teams_info");

            migrationBuilder.DropColumn(
                name: "test_records",
                table: "cricket_teams_info");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "odi_records",
                table: "cricket_teams_info",
                type: "jsonb",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "t20i_records",
                table: "cricket_teams_info",
                type: "jsonb",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "test_records",
                table: "cricket_teams_info",
                type: "jsonb",
                nullable: false,
                defaultValue: "");
        }
    }
}
