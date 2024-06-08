using AudioBlend.Core.Shared.Results;
using Microsoft.AspNetCore.Http;

namespace AudioBlend.Core.MusicData.Services.Interfaces
{
    public interface IAzureBlobStorageService
    {
        Task<Result<string>> UploadFileToBlobAsync(IFormFile file);
        Task<Result<string>> DeleteFileFromBlobAsync(string fileUri);
    }
}
