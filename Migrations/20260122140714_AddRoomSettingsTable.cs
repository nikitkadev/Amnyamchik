using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amnyam.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomSettingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "voice_room_name",
                table: "guild_member");

            migrationBuilder.CreateTable(
                name: "room_settings",
                columns: table => new
                {
                    GuildMemberDiscordId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    voiceroom_name = table.Column<string>(type: "text", nullable: true),
                    members_limit = table.Column<int>(type: "integer", nullable: true),
                    region = table.Column<string>(type: "text", nullable: true),
                    bitrate = table.Column<int>(type: "integer", nullable: true),
                    is_nsfw = table.Column<bool>(type: "boolean", nullable: true),
                    slowmode_limit = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room_settings", x => x.GuildMemberDiscordId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "room_settings");

            migrationBuilder.AddColumn<string>(
                name: "voice_room_name",
                table: "guild_member",
                type: "text",
                nullable: true);
        }
    }
}
