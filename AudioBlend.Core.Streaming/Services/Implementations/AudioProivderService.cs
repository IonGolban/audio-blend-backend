using AudioBlend.Core.Shared.Responses;
using AudioBlend.Core.Streaming.Services.Interfaces;

namespace AudioBlend.Core.Streaming.Services.Implementations
{
    public class AudioProivderService : IAudioProivderService
    {
        public Task<Response<Stream>> GetStreamAudioById(Guid stream)
        {
            throw new NotImplementedException();
        }

        public Task<Response<Stream>> GetStreamAudioDemo()
        {
            throw new NotImplementedException();
        }
    }
}
