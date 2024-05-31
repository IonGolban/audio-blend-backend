    using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AudioBlend.Core.MusicData.Migrations
{
    /// <inheritdoc />
    public partial class initWithPlaylistSongSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaylistSong",
                schema: "music_data");

            migrationBuilder.CreateTable(
                name: "playlist_songs",
                schema: "music_data",
                columns: table => new
                {
                    PlaylistId = table.Column<Guid>(type: "uuid", nullable: false),
                    SongId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_playlist_songs", x => new { x.PlaylistId, x.SongId });
                    table.ForeignKey(
                        name: "FK_playlist_songs_playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalSchema: "music_data",
                        principalTable: "playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_playlist_songs_songs_SongId",
                        column: x => x.SongId,
                        principalSchema: "music_data",
                        principalTable: "songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_playlist_songs_SongId",
                schema: "music_data",
                table: "playlist_songs",
                column: "SongId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "playlist_songs",
                schema: "music_data");

            migrationBuilder.CreateTable(
                name: "PlaylistSong",
                schema: "music_data",
                columns: table => new
                {
                    PlaylistsId = table.Column<Guid>(type: "uuid", nullable: false),
                    SongsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistSong", x => new { x.PlaylistsId, x.SongsId });
                    table.ForeignKey(
                        name: "FK_PlaylistSong_playlists_PlaylistsId",
                        column: x => x.PlaylistsId,
                        principalSchema: "music_data",
                        principalTable: "playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistSong_songs_SongsId",
                        column: x => x.SongsId,
                        principalSchema: "music_data",
                        principalTable: "songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistSong_SongsId",
                schema: "music_data",
                table: "PlaylistSong",
                column: "SongsId");
        }
    }
}
