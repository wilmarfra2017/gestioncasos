using GestionCasos.Domain.Dtos;
using GestionCasos.Domain.Entities;
using GestionCasos.Domain.Exceptions;
using GestionCasos.Domain.Ports;

namespace GestionCasos.Domain.Services;

[DomainService]
public class SolicitudService
{
    private readonly ISolicitudRepository _solicitudRepository;
    private readonly IReporteSolicitudService _reporteSolicitudService;
    private readonly ISolicitudPadreId _solicitudPadreId;
    private readonly ILogMessageService _resourceMessages;
    public SolicitudService(ISolicitudRepository solicitudRepository,
    IReporteSolicitudService reporteSolicitudService,
    ISolicitudPadreId solicitudPadreId,
    ILogMessageService resourceMessages
    )
    {
        _solicitudRepository = solicitudRepository;
        _reporteSolicitudService = reporteSolicitudService;
        _solicitudPadreId = solicitudPadreId;
        _resourceMessages = resourceMessages;
    }

    public async Task<Solicitud> GuardarSolicitudAsync(Solicitud solicitud)
    {
        VerificarSolicitud(solicitud,_resourceMessages.ErrorValidacionEntidadGuardar);
        return await _solicitudRepository.GuardarSolicitudAsync(solicitud);
    }
    public async Task<Solicitud> ActualizarSolicitudAsync(Solicitud solicitud)
    {
        VerificarSolicitud(solicitud,_resourceMessages.ErrorValidacionEntidadActualizar);
        return await _solicitudRepository.ActualizarSolicitudAsync(solicitud);
    }
    public async Task<ResponseSolicitudesCreadasDto> CrearSolicitudesAsync(IList<RequestSolicitudDto> solicitudes)
    {
        return await _reporteSolicitudService.CrearAsync(solicitudes);
    }
    public async Task<Solicitud> ConsultarSolicitudAsync(string idorkey)
    {
        return await _reporteSolicitudService.ConsultarAsync(idorkey)!;
    }

    public string ObtenerNombreSolicitudPadreId()
    {
        return _solicitudPadreId.ObtenerNombre()!;
    }

    private void VerificarSolicitud(Solicitud solicitud, string mensaje)
    {
        if (solicitud == null)
        {
            throw new ArgumentNullException(nameof(solicitud), _resourceMessages.ErrorValidacionEntidad);
        }
        if (!solicitud.ValidarSolicitud())
        {
            throw new GestionCasosException(mensaje);
        }
    }
}

