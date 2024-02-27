using Azure.Storage.Blobs;
using GestionCasos.Infrastructure.Config;
using GestionCasos.Infrastructure.Ports;
using System.Collections.Frozen;

namespace GestionCasos.Infrastructure.Adapters;

[Repository]
public class BlobStorageRepository : IBlobStorageRepository
{
    private readonly BlobServiceClient _blobServiceClient;
    public BlobStorageRepository(StorageConfig config)
    {
        string? connection = config.CandenaConexion;
        ArgumentException.ThrowIfNullOrEmpty(connection);
        _blobServiceClient = new(connection);
    }

    public async Task<Uri> UploadStreamAsync(Stream stream, string fileName, string containerName)
    {

        BlobContainerClient containerClient = await GetOrCreateBlobContainerClientAsync(containerName);
        BlobClient blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(stream,overwrite:true);

        return blobClient.Uri;
    }

    private async Task<BlobContainerClient> GetOrCreateBlobContainerClientAsync(string containerName)
    {
        var containers = _blobServiceClient.GetBlobContainers().ToFrozenSet();
        bool condition = containers is not null && containers.Any(c => c.Name.Equals(containerName, StringComparison.InvariantCultureIgnoreCase));
        if (condition)
        {
            return _blobServiceClient.GetBlobContainerClient(containerName);
        }

        return await _blobServiceClient.CreateBlobContainerAsync(containerName);
    }
}
