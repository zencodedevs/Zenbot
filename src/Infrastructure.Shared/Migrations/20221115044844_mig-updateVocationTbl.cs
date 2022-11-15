using Microsoft.EntityFrameworkCore.Migrations;

namespace Zenbot.Infrastructure.Shared.Migrations
{
    public partial class migupdateVocationTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Vocations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GuildId",
                table: "Vocations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Vocations_GuildId",
                table: "Vocations",
                column: "GuildId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vocations_Guilds_GuildId",
                table: "Vocations",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vocations_Guilds_GuildId",
                table: "Vocations");

            migrationBuilder.DropIndex(
                name: "IX_Vocations_GuildId",
                table: "Vocations");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Vocations");

            migrationBuilder.DropColumn(
                name: "GuildId",
                table: "Vocations");
        }
    }
}
