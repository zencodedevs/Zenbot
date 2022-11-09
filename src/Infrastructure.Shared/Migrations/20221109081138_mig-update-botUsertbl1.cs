using Microsoft.EntityFrameworkCore.Migrations;

namespace Zenbot.Infrastructure.Shared.Migrations
{
    public partial class migupdatebotUsertbl1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GuildName",
                table: "Guilds",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuildName",
                table: "Guilds");
        }
    }
}
