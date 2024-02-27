using GestionCasos.Domain.Dtos;
using GestionCasos.Domain.Entities;

namespace GestionCasos.Domain.Ports;

public interface IReporteSolicitudService
{
    Task<ResponseSolicitudesCreadasDto> CrearAsync(IList<RequestSolicitudDto> solicitudes);
    Task<Solicitud>? ConsultarAsync(string keyorid);
    Task<Solicitud> ActualizarAsync(string solicitudIdOrKey, RequestSolicitudDto solicitud);
}
