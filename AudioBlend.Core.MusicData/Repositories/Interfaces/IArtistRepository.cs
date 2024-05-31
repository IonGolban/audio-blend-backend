using AudioBlend.Core.MusicData.Domain.Artists;
using AudioBlend.Core.Repositories.Interfaces;

namespace AudioBlend.Core.MusicData.Repositories.Interfaces
{
    public interface IArtistRepository : IAsyncRepository<Artist>
    {
    }
}
