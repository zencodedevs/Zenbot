using Microsoft.EntityFrameworkCore.Migrations;

namespace Zenbot.Infrastructure.Shared.Migrations
{
    public partial class migbotUserUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnableIntegration",
                table: "BotUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnableIntegration",
                table: "BotUsers");
        }
    }
}
