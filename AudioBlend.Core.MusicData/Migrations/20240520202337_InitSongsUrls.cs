using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AudioBlend.Core.MusicData.Migrations
{
    /// <inheritdoc />
    public partial class InitSongsUrls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AudioUrl",
                schema: "music_data",
                table: "songs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AudioUrl",
                schema: "music_data",
                table: "songs");
        }
    }
}
