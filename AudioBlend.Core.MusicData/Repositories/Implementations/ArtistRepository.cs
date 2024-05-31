using AudioBlend.Core.MusicData.Domain.Artists;
using AudioBlend.Core.MusicData.Repositories.Interfaces;

namespace AudioBlend.Core.MusicData.Repositories.Implementations
{
    public class ArtistRepository(AudioBlendContext context) : BaseRepository<Artist>(context), IArtistRepository
    {
    }
}
