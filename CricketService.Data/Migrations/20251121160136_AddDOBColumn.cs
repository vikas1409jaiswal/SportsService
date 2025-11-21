using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CricketService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDOBColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "date_of_birth",
                table: "cricket_players_info",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date_of_birth",
                table: "cricket_players_info");
        }
    }
}
