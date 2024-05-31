using AudioBlend.Core.Shared.Results;

namespace AudioBlend.API.Services.Interfaces
{
    public interface IAzureBlobStorageService
    {
        Task<Result<string>> UploadFileToBlobAsync(IFormFile file);
        Task<Result<string>> DeleteFileFromBlobAsync(string fileUri);
    }
}
