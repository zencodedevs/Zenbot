using Microsoft.EntityFrameworkCore.Migrations;

namespace Zenbot.Infrastructure.Shared.Migrations
{
    public partial class migupdatebotUsertbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "BotUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "BotUserGuilds",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "BotUserGuilds");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "BotUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
