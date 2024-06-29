using Microsoft.AspNetCore.Mvc;
namespace AudioBlend.API.Controllers.AudioStreaming
{
    [Route("api/v1/audio")]
    public class AudioController : ControllerBase
    {
        //private readonly IAudioProivderService _audioProivderService;

        //public AudioController(IAudioProivderService audioProivderService)
        //{
        //    _audioProivderService = audioProivderService;
        //}

        //[HttpGet("demo")]
        //public async Task<IActionResult> GetDemoAudio()
        //{
        //    var res = await _audioProivderService.GetStreamAudioDemo();
        //    if (!res.Success)
        //    {
        //        return BadRequest(res.Message);
        //    }
        //    return File(res.Data, "audio/mpeg", enableRangeProcessing : true);
        //}

    }
}
