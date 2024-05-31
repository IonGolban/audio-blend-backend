using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AudioBlend.Core.MusicData.Migrations
{
    /// <inheritdoc />
    public partial class initwithUsersAndPlaylists23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string[]>(
                name: "LikedByUsers",
                schema: "music_data",
                table: "like_albums",
                type: "text[]",
                nullable: true,
                oldClrType: typeof(string[]),
                oldType: "text[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string[]>(
                name: "LikedByUsers",
                schema: "music_data",
                table: "like_albums",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0],
                oldClrType: typeof(string[]),
                oldType: "text[]",
                oldNullable: true);
        }
    }
}
