using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AudioBlend.Core.MusicData.Migrations
{
    /// <inheritdoc />
    public partial class AddedCoverUrlAlbum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikedByUsers",
                schema: "music_data",
                table: "like_albums");

            migrationBuilder.RenameColumn(
                name: "Popularity",
                schema: "music_data",
                table: "artists",
                newName: "Followers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Followers",
                schema: "music_data",
                table: "artists",
                newName: "Popularity");

            migrationBuilder.AddColumn<string[]>(
                name: "LikedByUsers",
                schema: "music_data",
                table: "like_albums",
                type: "text[]",
                nullable: true);
        }
    }
}
