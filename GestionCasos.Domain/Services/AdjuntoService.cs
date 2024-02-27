using GestionCasos.Domain.Entities;
using GestionCasos.Domain.Ports;

namespace GestionCasos.Domain.Services;

[DomainService]
public class AdjuntoService(IAdjuntoRepository _adjuntoRepository, ILogMessageService? _resourceManager)
{
    public Task<Adjunto> CargarAdjuntoAsync(string fileName, Stream fileStream, string formatoArchivo, string solicitudId)
    {
        return ValidarYAdjuntarAsync(fileName, fileStream, formatoArchivo, solicitudId);
    }
    private async Task<Adjunto> ValidarYAdjuntarAsync(string fileName, Stream fileStream, string formatoArchivo, string solicitudId)
    {
        var adjunto = new Adjunto();
        EjecutarValidaciones(adjunto, fileStream, formatoArchivo);
        ValidacionAdjunto(adjunto, _resourceManager);

        var adjuntoUrl = await _adjuntoRepository.CargarAdjuntoAsync(fileStream, solicitudId, fileName);
        adjunto.SetUrl(adjuntoUrl);
       
        return adjunto;
    }
    private static void ValidacionAdjunto(Adjunto adjunto, ILogMessageService? _resourceManager)
    {
        if (adjunto == null)
        {
            throw new ArgumentNullException(nameof(adjunto), _resourceManager!.ErrorValidacionAdjunto);
        }
    }

    private static void EjecutarValidaciones(Adjunto adjunto, Stream fileStream, string formatoArchivo)
    {
        adjunto.VerificarFormatoValido(formatoArchivo);
        Adjunto.VerificarStreamData(fileStream);
    }
}



