using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CricketService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCTHh2hTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cricket_teams_history_h2h",
                columns: table => new
                {
                    match_uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    team1_name = table.Column<string>(type: "text", nullable: false),
                    team2_name = table.Column<string>(type: "text", nullable: false),
                    match_number = table.Column<int>(type: "integer", nullable: false),
                    instant_teams_records = table.Column<string>(type: "jsonb", nullable: false),
                    format = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cricket_teams_history_h2h", x => x.match_uuid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cricket_teams_history_h2h");
        }
    }
}
