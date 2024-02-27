using GestionCasos.Domain.Entities;

namespace GestionCasos.Domain.Ports;

public interface IEstructuraSmsService
{
    Task<EstructuraSmsNotificacion> CrearEstructuraSmsAsync(string numero, string comentario, string estado, string nombreIntermediario, string SolicitudKey);
}
