﻿// <auto-generated />
using System;
using System.Collections.Generic;
using AudioBlend.Core.MusicData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AudioBlend.Core.MusicData.Migrations
{
    [DbContext(typeof(AudioBlendContext))]
    [Migration("20240527112856_AddedCoverUrlAlbum")]
    partial class AddedCoverUrlAlbum
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("music_data")
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AudioBlend.Core.MusicData.Domain.Albums.Album", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ArtistId")
                        .HasColumnType("uuid");

                    b.Property<string>("CoverUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.ToTable("albums", "music_data");
                });

            modelBuilder.Entity("AudioBlend.Core.MusicData.Domain.Artists.Artist", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Followers")
                        .HasColumnType("integer");

                    b.Property<List<string>>("Genres")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("ImgUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("artists", "music_data");
                });

            modelBuilder.Entity("AudioBlend.Core.MusicData.Domain.Playlists.Playlist", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("boolean");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("playlists", "music_data");
                });

            modelBuilder.Entity("AudioBlend.Core.MusicData.Domain.Songs.Song", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AlbumId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ArtistId")
                        .HasColumnType("uuid");

                    b.Property<string>("AudioUrl")
                        .HasColumnType("text");

                    b.Property<int>("Duration")
                        .HasColumnType("integer");

                    b.Property<List<string>>("Genres")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AlbumId");

                    b.HasIndex("ArtistId");

                    b.ToTable("songs", "music_data");
                });

            modelBuilder.Entity("AudioBlend.Core.MusicData.Models.Likes.LikeAlbum", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<Guid>("AlbumId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "AlbumId");

                    b.HasIndex("AlbumId");

                    b.ToTable("like_albums", "music_data");
                });

            modelBuilder.Entity("AudioBlend.Core.MusicData.Models.Likes.LikePlaylist", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<Guid>("PlaylistId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "PlaylistId");

                    b.HasIndex("PlaylistId");

                    b.ToTable("like_playlists", "music_data");
                });

            modelBuilder.Entity("AudioBlend.Core.MusicData.Models.Likes.LikeSong", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<Guid>("SongId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "SongId");

                    b.HasIndex("SongId");

                    b.ToTable("like_songs", "music_data");
                });

            modelBuilder.Entity("AudioBlend.Core.MusicData.Models.Playlists.PlaylistSong", b =>
                {
                    b.Property<Guid>("PlaylistId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SongId")
                        .HasColumnType("uuid");

                    b.HasKey("PlaylistId", "SongId");

                    b.HasIndex("SongId");

                    b.ToTable("playlist_songs", "music_data");
                });

            modelBuilder.Entity("AudioBlend.Core.MusicData.Domain.Albums.Album", b =>
                {
                    b.HasOne("AudioBlend.Core.MusicData.Domain.Artists.Artist", "Artist")
                        .WithMany()
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");
                });

            modelBuilder.Entity("AudioBlend.Core.MusicData.Domain.Songs.Song", b =>
                {
                    b.HasOne("AudioBlend.Core.MusicData.Domain.Albums.Album", "Album")
                        .WithMany("Songs")
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AudioBlend.Core.MusicData.Domain.Artists.Artist", "Artist")
                        .WithMany()
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Album");

                    b.Navigation("Artist");
                });

            modelBuilder.Entity("AudioBlend.Core.MusicData.Models.Likes.LikeAlbum", b =>
                {
                    b.HasOne("AudioBlend.Core.MusicData.Domain.Albums.Album", "Album")
                        .WithMany("LikedByUsers")
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Album");
                });

            modelBuilder.Entity("AudioBlend.Core.MusicData.Models.Likes.LikePlaylist", b =>
                {
                    b.HasOne("AudioBlend.Core.MusicData.Domain.Playlists.Playlist", "Playlist")
                        .WithMany("LikedByUsers")
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Playlist");
                });

            modelBuilder.Entity("AudioBlend.Core.MusicData.Models.Likes.LikeSong", b =>
                {
                    b.HasOne("AudioBlend.Core.MusicData.Domain.Songs.Song", "Song")
                        .WithMany("LikedByUsers")
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Song");
                });

            modelBuilder.Entity("AudioBlend.Core.MusicData.Models.Playlists.PlaylistSong", b =>
                {
                    b.HasOne("AudioBlend.Core.MusicData.Domain.Playlists.Playlist", "Playlist")
                        .WithMany("playlistSongs")
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AudioBlend.Core.MusicData.Domain.Songs.Song", "Song")
                        .WithMany()
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Playlist");

                    b.Navigation("Song");
                });

            modelBuilder.Entity("AudioBlend.Core.MusicData.Domain.Albums.Album", b =>
                {
                    b.Navigation("LikedByUsers");

                    b.Navigation("Songs");
                });

            modelBuilder.Entity("AudioBlend.Core.MusicData.Domain.Playlists.Playlist", b =>
                {
                    b.Navigation("LikedByUsers");

                    b.Navigation("playlistSongs");
                });

            modelBuilder.Entity("AudioBlend.Core.MusicData.Domain.Songs.Song", b =>
                {
                    b.Navigation("LikedByUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
