using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amnyam.Migrations
{
    /// <inheritdoc />
    public partial class DeleteBitrateColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bitrate",
                table: "room_settings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "bitrate",
                table: "room_settings",
                type: "integer",
                nullable: true);
        }
    }
}
