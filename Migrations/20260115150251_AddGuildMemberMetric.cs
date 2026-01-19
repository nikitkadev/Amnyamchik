using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amnyam.Migrations
{
    /// <inheritdoc />
    public partial class AddGuildMemberMetric : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "starting_at",
                table: "guild_voice_session",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<decimal>(
                name: "GuildMemberDiscordId",
                table: "guild_voice_session",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ending_at",
                table: "guild_voice_session",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "guild_messages",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "guilb_member_metrics",
                columns: table => new
                {
                    MemberDiscordId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    messages_sent_count = table.Column<int>(type: "integer", nullable: false),
                    reaction_added_count = table.Column<int>(type: "integer", nullable: false),
                    commands_sent_count = table.Column<int>(type: "integer", nullable: false),
                    stickers_sent_count = table.Column<int>(type: "integer", nullable: false),
                    gifs_sent_count = table.Column<int>(type: "integer", nullable: false),
                    pngpictures_sent_count = table.Column<int>(type: "integer", nullable: false),
                    first_message_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    last_message_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_guilb_member_metrics", x => x.MemberDiscordId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "guilb_member_metrics");

            migrationBuilder.DropColumn(
                name: "GuildMemberDiscordId",
                table: "guild_voice_session");

            migrationBuilder.DropColumn(
                name: "ending_at",
                table: "guild_voice_session");

            migrationBuilder.AlterColumn<DateTime>(
                name: "starting_at",
                table: "guild_voice_session",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "guild_messages",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
