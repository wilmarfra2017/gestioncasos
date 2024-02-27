namespace GestionCasos.Infrastructure.Ports;

public interface IBlobStorageRepository
{
    Task<Uri> UploadStreamAsync(Stream stream, string fileName, string containerName);
}
