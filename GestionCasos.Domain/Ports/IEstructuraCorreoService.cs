using GestionCasos.Domain.Entities;

namespace GestionCasos.Domain.Ports;

public interface IEstructuraCorreoService
{
    Task<EstructuraCorreoNotificacion> CrearEstructuraCorreoAsync(string email, string comentario, string estado, string nombreIntermediario, string SolicitudKey);
}
