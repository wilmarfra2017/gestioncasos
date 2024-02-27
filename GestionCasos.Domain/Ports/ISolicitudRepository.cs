using GestionCasos.Domain.Entities;

namespace GestionCasos.Domain.Ports;

public interface ISolicitudRepository
{
    Task<Solicitud> GuardarSolicitudAsync(Solicitud solicitud);
    Task<Solicitud> ActualizarSolicitudAsync(Solicitud solicitud);
}
