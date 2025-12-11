using Azure.Storage.Blobs;
using Core.Interface.Services;

namespace Core.Interfaces.Repositories;

public class AzurePlantImageService : IPlantImageService
{
    private readonly BlobContainerClient _containerClient;

    public AzurePlantImageService(BlobContainerClient containerClient)
    {
        _containerClient = containerClient;
    }

    public async Task<string> SavePlantImageAsync(Stream content, string fileName, string contentType, CancellationToken token)
    {
        var extension = Path.GetExtension(fileName);
        if (string.IsNullOrWhiteSpace(extension))
        {
            extension = ".jpg";
        }

        var blobName = $"plants/{Guid.NewGuid()}{extension}";
        var blobClient = _containerClient.GetBlobClient(blobName);

        await blobClient.UploadAsync(content, overwrite: true, token);

        return blobClient.Uri.ToString();
    }
}   