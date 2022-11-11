using Microsoft.EntityFrameworkCore.Migrations;

namespace Zenbot.Infrastructure.Shared.Migrations
{
    public partial class migboardingMessage_file : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BoardingMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    GuildId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardingMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoardingMessages_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BoardingFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BoardingMessageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardingFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoardingFiles_BoardingMessages_BoardingMessageId",
                        column: x => x.BoardingMessageId,
                        principalTable: "BoardingMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardingFiles_BoardingMessageId",
                table: "BoardingFiles",
                column: "BoardingMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_BoardingMessages_GuildId",
                table: "BoardingMessages",
                column: "GuildId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardingFiles");

            migrationBuilder.DropTable(
                name: "BoardingMessages");
        }
    }
}
