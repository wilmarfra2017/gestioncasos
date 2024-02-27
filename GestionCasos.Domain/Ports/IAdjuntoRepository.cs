namespace GestionCasos.Domain.Ports;

public interface IAdjuntoRepository
{
    Task<Uri> CargarAdjuntoAsync(Stream stream, string solicitudId, string nombreAdjunto);
}
