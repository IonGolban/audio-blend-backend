using AudioBlend.API.Services.Interfaces;
using AudioBlend.Core.Shared.Results;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AudioBlend.API.Services.Implementations
{
    public class AzureBlobStorageService : IAzureBlobStorageService
    {
        private readonly IConfiguration _configuration;

        public AzureBlobStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Result<string>> UploadFileToBlobAsync(IFormFile file)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("AzureBlobStorage");
                var container = "default";

                var blobName = $"{Guid.NewGuid()}_{file.FileName}";

                var blobContainerClient = new BlobContainerClient(connectionString, container);
                var blobClient = blobContainerClient.GetBlobClient(blobName);

                await blobClient.UploadAsync(file.OpenReadStream(), new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders { ContentType = file.ContentType }
                });

                await blobContainerClient.SetAccessPolicyAsync(PublicAccessType.Blob);

                return Result<string>.Success(blobClient.Uri.AbsoluteUri);
            }
            catch (Exception ex)
            {
                return Result<string>.Failure($"Error while uploading file to blob: {ex.Message}");
            }
        }

        public async Task<Result<string>> DeleteFileFromBlobAsync(string fileUri)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("AzureBlobStorage");
                var containerName = _configuration["AzureBlobStorageContainerName"];

                var containerClient = new BlobContainerClient(connectionString, containerName);
                var fileName = Path.GetFileName(fileUri);
                var blobClient = containerClient.GetBlobClient(fileName);
                await blobClient.DeleteIfExistsAsync();
                return Result<string>.Success(fileName);
            }
            catch (Exception ex)
            {
                return Result<string>.Failure($"Error while deleting file from blob: {ex.Message}");
            }
        }

    }
}
