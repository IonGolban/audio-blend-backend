using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AudioBlend.Core.MusicData.Migrations
{
    /// <inheritdoc />
    public partial class followArtistMigr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_follow_artists_ArtistId",
                schema: "music_data",
                table: "follow_artists",
                column: "ArtistId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "follow_artists",
                schema: "music_data");
        }
    }
}
