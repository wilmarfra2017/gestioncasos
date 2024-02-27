using GestionCasos.Domain.Ports;
using GestionCasos.Infrastructure.Ports;

namespace GestionCasos.Infrastructure.Adapters;

[Repository]
public class AdjuntoRepository : IAdjuntoRepository
{
    private readonly IBlobStorageRepository _blobStorageRepository;

    public AdjuntoRepository(IBlobStorageRepository blobStorageRepository)
    {
        _blobStorageRepository = blobStorageRepository ?? throw new ArgumentNullException(nameof(blobStorageRepository));
    }

    public async Task<Uri> CargarAdjuntoAsync(Stream stream, string solicitudId, string nombreAdjunto)
    {
        return await _blobStorageRepository.UploadStreamAsync(
            stream: stream,
            fileName: nombreAdjunto,
            containerName: solicitudId
        );
    }
}
