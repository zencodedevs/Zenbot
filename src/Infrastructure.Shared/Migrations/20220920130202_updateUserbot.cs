using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Shared.Migrations
{
    public partial class updateUserbot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JiraAccountID",
                table: "BotUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JiraAccountID",
                table: "BotUsers");
        }
    }
}
