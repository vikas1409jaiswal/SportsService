using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CricketService.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveExtraPlayersInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "contents",
                table: "cricket_players_info");

            migrationBuilder.DropColumn(
                name: "date_of_birth",
                table: "cricket_players_info");

            migrationBuilder.DropColumn(
                name: "date_of_death",
                table: "cricket_players_info");

            migrationBuilder.DropColumn(
                name: "debut_details",
                table: "cricket_players_info");

            migrationBuilder.DropColumn(
                name: "extra_info",
                table: "cricket_players_info");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "contents",
                table: "cricket_players_info",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "date_of_birth",
                table: "cricket_players_info",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "date_of_death",
                table: "cricket_players_info",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "debut_details",
                table: "cricket_players_info",
                type: "jsonb",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "extra_info",
                table: "cricket_players_info",
                type: "jsonb",
                nullable: false,
                defaultValue: "");
        }
    }
}
