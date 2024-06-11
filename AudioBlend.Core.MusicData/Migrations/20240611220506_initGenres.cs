using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AudioBlend.Core.MusicData.Migrations
{
    /// <inheritdoc />
    public partial class initGenres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "music_data");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:fuzzystrmatch", ",,");

            migrationBuilder.CreateTable(
                name: "artists",
                schema: "music_data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ImgUrl = table.Column<string>(type: "text", nullable: false),
                    GenresIds = table.Column<List<Guid>>(type: "uuid[]", nullable: false),
                    Followers = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_artists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "genres",
                schema: "music_data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "playlists",
                schema: "music_data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CoverUrl = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Likes = table.Column<int>(type: "integer", nullable: false)
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
                    CoverUrl = table.Column<string>(type: "text", nullable: true),
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
                name: "follow_artists",
                schema: "music_data",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ArtistId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_follow_artists", x => new { x.UserId, x.ArtistId });
                    table.ForeignKey(
                        name: "FK_follow_artists_artists_ArtistId",
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
                    AlbumId = table.Column<Guid>(type: "uuid", nullable: false)
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
                    GenresIds = table.Column<List<Guid>>(type: "uuid[]", nullable: false),
                    ArtistId = table.Column<Guid>(type: "uuid", nullable: false),
                    AlbumId = table.Column<Guid>(type: "uuid", nullable: false),
                    AudioUrl = table.Column<string>(type: "text", nullable: true)
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
                name: "IX_albums_ArtistId",
                schema: "music_data",
                table: "albums",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_follow_artists_ArtistId",
                schema: "music_data",
                table: "follow_artists",
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
                name: "IX_playlist_songs_SongId",
                schema: "music_data",
                table: "playlist_songs",
                column: "SongId");

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
                name: "follow_artists",
                schema: "music_data");

            migrationBuilder.DropTable(
                name: "genres",
                schema: "music_data");

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
                name: "playlist_songs",
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
