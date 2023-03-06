using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CricketService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCricketTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cricket_players_info",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    full_name = table.Column<string>(type: "text", nullable: false),
                    href = table.Column<string>(type: "text", nullable: false),
                    team_name = table.Column<string>(type: "text", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    birth_place = table.Column<string>(type: "text", nullable: false),
                    formats = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cricket_players_info", x => x.uuid);
                });

            migrationBuilder.CreateTable(
                name: "one_day_international_matches",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    season = table.Column<string>(type: "text", nullable: false),
                    series = table.Column<string>(type: "text", nullable: false),
                    player_of_the_match = table.Column<string>(type: "text", nullable: false),
                    match_no = table.Column<string>(type: "text", nullable: false),
                    match_days = table.Column<string>(type: "text", nullable: false),
                    match_title = table.Column<string>(type: "text", nullable: false),
                    venue = table.Column<string>(type: "text", nullable: false),
                    match_date = table.Column<string>(type: "text", nullable: false),
                    toss_winner = table.Column<string>(type: "text", nullable: false),
                    toss_decision = table.Column<string>(type: "text", nullable: false),
                    result = table.Column<string>(type: "text", nullable: false),
                    team1_details = table.Column<string>(type: "jsonb", nullable: false),
                    team2_details = table.Column<string>(type: "jsonb", nullable: false),
                    tv_umpire = table.Column<string>(type: "text", nullable: false),
                    match_referee = table.Column<string>(type: "text", nullable: false),
                    reserve_umpire = table.Column<string>(type: "text", nullable: false),
                    umpires = table.Column<string[]>(type: "text[]", nullable: false),
                    format_debut = table.Column<string[]>(type: "text[]", nullable: false),
                    international_debut = table.Column<string[]>(type: "text[]", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_one_day_international_matches", x => x.uuid);
                });

            migrationBuilder.CreateTable(
                name: "t20_international_matches",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    season = table.Column<string>(type: "text", nullable: false),
                    series = table.Column<string>(type: "text", nullable: false),
                    player_of_the_match = table.Column<string>(type: "text", nullable: false),
                    match_no = table.Column<string>(type: "text", nullable: false),
                    match_days = table.Column<string>(type: "text", nullable: false),
                    match_title = table.Column<string>(type: "text", nullable: false),
                    venue = table.Column<string>(type: "text", nullable: false),
                    match_date = table.Column<string>(type: "text", nullable: false),
                    toss_winner = table.Column<string>(type: "text", nullable: false),
                    toss_decision = table.Column<string>(type: "text", nullable: false),
                    result = table.Column<string>(type: "text", nullable: false),
                    team1_details = table.Column<string>(type: "jsonb", nullable: false),
                    team2_details = table.Column<string>(type: "jsonb", nullable: false),
                    tv_umpire = table.Column<string>(type: "text", nullable: false),
                    match_referee = table.Column<string>(type: "text", nullable: false),
                    reserve_umpire = table.Column<string>(type: "text", nullable: false),
                    umpires = table.Column<string[]>(type: "text[]", nullable: false),
                    format_debut = table.Column<string[]>(type: "text[]", nullable: false),
                    international_debut = table.Column<string[]>(type: "text[]", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t20_international_matches", x => x.uuid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cricket_players_info");

            migrationBuilder.DropTable(
                name: "one_day_international_matches");

            migrationBuilder.DropTable(
                name: "t20_international_matches");
        }
    }
}
