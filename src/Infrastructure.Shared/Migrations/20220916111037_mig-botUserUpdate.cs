using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Shared.Migrations
{
    public partial class migbotUserUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "BotUsers");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "BotUsers");

            migrationBuilder.AlterColumn<decimal>(
                name: "UserId",
                table: "BotUsers",
                type: "decimal(20,0)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Birthday",
                table: "BotUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "NextNotifyTIme",
                table: "BotUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "BotUsers");

            migrationBuilder.DropColumn(
                name: "NextNotifyTIme",
                table: "BotUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BotUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,0)");

            migrationBuilder.AddColumn<byte>(
                name: "Day",
                table: "BotUsers",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "Month",
                table: "BotUsers",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
