using Microsoft.EntityFrameworkCore.Migrations;

namespace Zenbot.Infrastructure.Shared.Migrations
{
    public partial class migbitbucket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "BotUsers",
                newName: "DiscordUserId");

            migrationBuilder.AddColumn<string>(
                name: "BitBucketAccountId",
                table: "BotUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BitBucketAccountId",
                table: "BotUsers");

            migrationBuilder.RenameColumn(
                name: "DiscordUserId",
                table: "BotUsers",
                newName: "UserId");
        }
    }
}
