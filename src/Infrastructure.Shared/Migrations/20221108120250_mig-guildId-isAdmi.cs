using Microsoft.EntityFrameworkCore.Migrations;

namespace Zenbot.Infrastructure.Shared.Migrations
{
    public partial class migguildIdisAdmi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guilds_AspNetUsers_UserId",
                table: "Guilds");

            migrationBuilder.DropIndex(
                name: "IX_Guilds_UserId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "BotUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Guilds",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "BotUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_UserId",
                table: "Guilds",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guilds_AspNetUsers_UserId",
                table: "Guilds",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
