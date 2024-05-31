
using AudioBlend.Core.Shared.Responses;

namespace AudioBlend.Core.Streaming.Services.Interfaces
{
    public interface IAudioProivderService
    {
        Task<Response<Stream>> GetStreamAudioById(Guid stream);
        Task<Response<Stream>> GetStreamAudioDemo();

    }
}
