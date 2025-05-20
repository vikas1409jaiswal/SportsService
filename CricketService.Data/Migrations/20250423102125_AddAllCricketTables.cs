using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CricketService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAllCricketTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cricket_players_info",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    full_name = table.Column<string>(type: "text", nullable: false),
                    href = table.Column<string>(type: "text", nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: false),
                    international_team_names = table.Column<string>(type: "text", nullable: false),
                    team_names = table.Column<string>(type: "text", nullable: false),
                    date_of_birth = table.Column<string>(type: "jsonb", nullable: true),
                    date_of_death = table.Column<string>(type: "jsonb", nullable: true),
                    debut_details = table.Column<string>(type: "jsonb", nullable: false),
                    formats = table.Column<string>(type: "text", nullable: false),
                    extra_info = table.Column<string>(type: "jsonb", nullable: false),
                    contents = table.Column<string[]>(type: "text[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cricket_players_info", x => x.uuid);
                });

            migrationBuilder.CreateTable(
                name: "cricket_teams_history",
                columns: table => new
                {
                    match_uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    match_number = table.Column<int>(type: "integer", nullable: false),
                    instant_teams_records = table.Column<string>(type: "jsonb", nullable: false),
                    format = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cricket_teams_history", x => x.match_uuid);
                });

            migrationBuilder.CreateTable(
                name: "cricket_teams_info",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    team_name = table.Column<string>(type: "text", nullable: false),
                    formats = table.Column<string>(type: "text", nullable: false),
                    logo_url = table.Column<string>(type: "text", nullable: false),
                    flag_url = table.Column<string>(type: "text", nullable: false),
                    test_records = table.Column<string>(type: "jsonb", nullable: false),
                    odi_records = table.Column<string>(type: "jsonb", nullable: false),
                    t20i_records = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cricket_teams_info", x => x.uuid);
                });

            migrationBuilder.CreateTable(
                name: "limited_over_international_matches",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    team1_details = table.Column<string>(type: "jsonb", nullable: false),
                    team2_details = table.Column<string>(type: "jsonb", nullable: false),
                    tv_umpire = table.Column<string>(type: "text", nullable: false),
                    match_referee = table.Column<string>(type: "text", nullable: false),
                    reserve_umpire = table.Column<string>(type: "text", nullable: false),
                    umpires = table.Column<string[]>(type: "text[]", nullable: false),
                    international_debut = table.Column<string>(type: "jsonb", nullable: false),
                    match_number = table.Column<string>(type: "text", nullable: false),
                    match_type = table.Column<string>(type: "text", nullable: false),
                    match_date = table.Column<string>(type: "text", nullable: false),
                    season = table.Column<string>(type: "text", nullable: false),
                    series = table.Column<string>(type: "text", nullable: false),
                    series_result = table.Column<string>(type: "text", nullable: true),
                    match_title = table.Column<string>(type: "text", nullable: false),
                    player_of_the_match = table.Column<string>(type: "jsonb", nullable: true),
                    toss_winner = table.Column<string>(type: "text", nullable: false),
                    toss_decision = table.Column<string>(type: "text", nullable: false),
                    result = table.Column<string>(type: "text", nullable: false),
                    venue = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_limited_over_international_matches", x => x.uuid);
                });

            migrationBuilder.CreateTable(
                name: "t20_matches",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    team1_details = table.Column<string>(type: "jsonb", nullable: false),
                    team2_details = table.Column<string>(type: "jsonb", nullable: false),
                    tv_umpire = table.Column<string>(type: "text", nullable: false),
                    match_referee = table.Column<string>(type: "text", nullable: false),
                    reserve_umpire = table.Column<string>(type: "text", nullable: false),
                    umpires = table.Column<string[]>(type: "text[]", nullable: false),
                    format_debut = table.Column<string>(type: "jsonb", nullable: false),
                    match_number = table.Column<string>(type: "text", nullable: false),
                    match_type = table.Column<string>(type: "text", nullable: false),
                    match_date = table.Column<string>(type: "text", nullable: false),
                    season = table.Column<string>(type: "text", nullable: false),
                    series = table.Column<string>(type: "text", nullable: false),
                    series_result = table.Column<string>(type: "text", nullable: true),
                    match_title = table.Column<string>(type: "text", nullable: false),
                    player_of_the_match = table.Column<string>(type: "jsonb", nullable: true),
                    toss_winner = table.Column<string>(type: "text", nullable: false),
                    toss_decision = table.Column<string>(type: "text", nullable: false),
                    result = table.Column<string>(type: "text", nullable: false),
                    venue = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t20_matches", x => x.uuid);
                });

            migrationBuilder.CreateTable(
                name: "test_cricket_matches",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    team1_details = table.Column<string>(type: "jsonb", nullable: false),
                    team2_details = table.Column<string>(type: "jsonb", nullable: false),
                    tv_umpire = table.Column<string>(type: "text", nullable: false),
                    match_referee = table.Column<string>(type: "text", nullable: false),
                    reserve_umpire = table.Column<string>(type: "text", nullable: false),
                    umpires = table.Column<string[]>(type: "text[]", nullable: false),
                    international_debut = table.Column<string>(type: "jsonb", nullable: false),
                    match_number = table.Column<string>(type: "text", nullable: false),
                    match_type = table.Column<string>(type: "text", nullable: false),
                    match_date = table.Column<string>(type: "text", nullable: false),
                    season = table.Column<string>(type: "text", nullable: false),
                    series = table.Column<string>(type: "text", nullable: false),
                    series_result = table.Column<string>(type: "text", nullable: true),
                    match_title = table.Column<string>(type: "text", nullable: false),
                    player_of_the_match = table.Column<string>(type: "jsonb", nullable: true),
                    toss_winner = table.Column<string>(type: "text", nullable: false),
                    toss_decision = table.Column<string>(type: "text", nullable: false),
                    result = table.Column<string>(type: "text", nullable: false),
                    venue = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_test_cricket_matches", x => x.uuid);
                });

            migrationBuilder.CreateTable(
                name: "cricket_teams_and_players_info",
                columns: table => new
                {
                    team_uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    player_uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    team_name = table.Column<string>(type: "text", nullable: false),
                    player_name = table.Column<string>(type: "text", nullable: false),
                    career_statistics = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cricket_teams_and_players_info", x => new { x.team_uuid, x.player_uuid });
                    table.ForeignKey(
                        name: "FK_cricket_teams_and_players_info_cricket_players_info_player_~",
                        column: x => x.player_uuid,
                        principalTable: "cricket_players_info",
                        principalColumn: "uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cricket_teams_and_players_info_cricket_teams_info_team_uuid",
                        column: x => x.team_uuid,
                        principalTable: "cricket_teams_info",
                        principalColumn: "uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cricket_teams_and_players_info_player_uuid",
                table: "cricket_teams_and_players_info",
                column: "player_uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cricket_teams_and_players_info");

            migrationBuilder.DropTable(
                name: "cricket_teams_history");

            migrationBuilder.DropTable(
                name: "limited_over_international_matches");

            migrationBuilder.DropTable(
                name: "t20_matches");

            migrationBuilder.DropTable(
                name: "test_cricket_matches");

            migrationBuilder.DropTable(
                name: "cricket_players_info");

            migrationBuilder.DropTable(
                name: "cricket_teams_info");
        }
    }
}
