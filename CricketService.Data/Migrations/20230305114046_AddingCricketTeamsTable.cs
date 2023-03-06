using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CricketService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingCricketTeamsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cricket_teams_info",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    team_name = table.Column<string>(type: "text", nullable: false),
                    formats = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cricket_teams_info", x => x.uuid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cricket_teams_info");
        }
    }
}
