using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AudioBlend.Core.MusicData.Migrations
{
    /// <inheritdoc />
    public partial class MusicDataSeed1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "music_data");

            migrationBuilder.CreateTable(
                name: "artists",
                schema: "music_data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ImgUrl = table.Column<string>(type: "text", nullable: false),
                    Genres = table.Column<List<string>>(type: "text[]", nullable: false),
                    Popularity = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_artists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "playlists",
                schema: "music_data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_playlists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "albums",
                schema: "music_data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    CoverUrl = table.Column<string>(type: "text", nullable: false),
                    ArtistId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_albums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_albums_artists_ArtistId",
                        column: x => x.ArtistId,
                        principalSchema: "music_data",
                        principalTable: "artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "like_playlists",
                schema: "music_data",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    PlaylistId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_like_playlists", x => new { x.UserId, x.PlaylistId });
                    table.ForeignKey(
                        name: "FK_like_playlists_playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalSchema: "music_data",
                        principalTable: "playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "like_albums",
                schema: "music_data",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    AlbumId = table.Column<Guid>(type: "uuid", nullable: false),
                    LikedByUsers = table.Column<string[]>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_like_albums", x => new { x.UserId, x.AlbumId });
                    table.ForeignKey(
                        name: "FK_like_albums_albums_AlbumId",
                        column: x => x.AlbumId,
                        principalSchema: "music_data",
                        principalTable: "albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "songs",
                schema: "music_data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    Genres = table.Column<List<string>>(type: "text[]", nullable: false),
                    ArtistId = table.Column<Guid>(type: "uuid", nullable: false),
                    AlbumId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_songs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_songs_albums_AlbumId",
                        column: x => x.AlbumId,
                        principalSchema: "music_data",
                        principalTable: "albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_songs_artists_ArtistId",
                        column: x => x.ArtistId,
                        principalSchema: "music_data",
                        principalTable: "artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "like_songs",
                schema: "music_data",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    SongId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_like_songs", x => new { x.UserId, x.SongId });
                    table.ForeignKey(
                        name: "FK_like_songs_songs_SongId",
                        column: x => x.SongId,
                        principalSchema: "music_data",
                        principalTable: "songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_albums_ArtistId",
                schema: "music_data",
                table: "albums",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_like_albums_AlbumId",
                schema: "music_data",
                table: "like_albums",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_like_playlists_PlaylistId",
                schema: "music_data",
                table: "like_playlists",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_like_songs_SongId",
                schema: "music_data",
                table: "like_songs",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistSong_SongsId",
                schema: "music_data",
                table: "PlaylistSong",
                column: "SongsId");

            migrationBuilder.CreateIndex(
                name: "IX_songs_AlbumId",
                schema: "music_data",
                table: "songs",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_songs_ArtistId",
                schema: "music_data",
                table: "songs",
                column: "ArtistId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "like_albums",
                schema: "music_data");

            migrationBuilder.DropTable(
                name: "like_playlists",
                schema: "music_data");

            migrationBuilder.DropTable(
                name: "like_songs",
                schema: "music_data");

            migrationBuilder.DropTable(
                name: "PlaylistSong",
                schema: "music_data");

            migrationBuilder.DropTable(
                name: "playlists",
                schema: "music_data");

            migrationBuilder.DropTable(
                name: "songs",
                schema: "music_data");

            migrationBuilder.DropTable(
                name: "albums",
                schema: "music_data");

            migrationBuilder.DropTable(
                name: "artists",
                schema: "music_data");
        }
    }
}
