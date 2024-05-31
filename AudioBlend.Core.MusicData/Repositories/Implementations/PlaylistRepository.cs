﻿using AudioBlend.Core.MusicData.Domain.Playlists;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace AudioBlend.Core.MusicData.Repositories.Implementations
{
    public class PlaylistRepository(AudioBlendContext context) : BaseRepository<Playlist>(context), IPlaylistRepository
    {
        private readonly AudioBlendContext _context = context;

        public async Task<Result<List<Playlist>>> GetPlaylistsByUserId(string userId)
        {
            var playlists = await _context.Playlists
                  .Include(p => p.LikedByUsers)
                  .Include(p => p.playlistSongs)
                    .ThenInclude(ps => ps.Song)
                  .Where(p => p.UserId == userId).ToListAsync();
            if (playlists == null)
            {
                return Result<List<Playlist>>.Failure($"Playlists with user id = {userId} not found");
            }

            return Result<List<Playlist>>.Success(playlists);

        }
        public async Task<Result<List<Playlist>>> GetLikedByUserId(string userId)
        {
            var playlists = await _context.LikePlaylists
                .Where(lp => lp.UserId == userId)
                .Include(lp => lp.Playlist)
                .ThenInclude(p => p.playlistSongs)
                .ThenInclude(ps => ps.Song)
                .ToListAsync();

            if (playlists == null)
            {
                return Result<List<Playlist>>.Failure("No liked playlists found");
            }

            return Result<List<Playlist>>.Success(playlists.Select(lp => lp.Playlist).ToList());

    
        }

    }
}
