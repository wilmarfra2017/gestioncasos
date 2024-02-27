using GestionCasos.Domain.Ports;
using GestionCasos.Domain.Services;
using MediatR;
using GestionCasos.Domain.Exceptions;
using GestionCasos.Domain.Dtos;
using GestionCasos.Domain.Entities;

namespace GestionCasos.Application.Features.Solicitudes.Commands.ActualizarSolicitud;
public class ActualizarSolicitudCommandHandler(
                                                SolicitudService _solicitudService,
                                                ISolicitudQueriesRepository _solicitudRepository,
                                                IReporteSolicitudService _reporteSolicitudService,
                                                ILogMessageService _resourceManager
                                            ) : IRequestHandler<ActualizarSolicitudCommand, bool>
{
    public Task<bool> Handle(ActualizarSolicitudCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return HandleAsync(request);
    }
    private async Task<bool> HandleAsync(ActualizarSolicitudCommand request)
    {
        var solicitud = await ActualizarSolicitudEnJiraAsync(request);
        await ActualizarSolicitudEnBaseDeDatosAsync(solicitud);
        return true;
    }
    private async Task<Solicitud> ActualizarSolicitudEnJiraAsync(ActualizarSolicitudCommand request)
    {
        var solicitudInfo = await _reporteSolicitudService.ConsultarAsync(request.SolicitudId)!;
        if (solicitudInfo.SolicitudId is null)
        {
            throw new GestionCasosException($"{_resourceManager.ErrorValidacionEstadoSolicitud}: {request.SolicitudId}");
        }

        var solicitudRequest = new RequestSolicitudDto
        {
            Fields = new Fields
            {
                Customfields = request.Campos
            }
        };

        return await _reporteSolicitudService.ActualizarAsync(request.SolicitudId, solicitudRequest);
    }
    private async Task<Solicitud> ActualizarSolicitudEnBaseDeDatosAsync(Solicitud solicitud)
    {
        var solicitudActual = await _solicitudRepository.ObtenerPorSolicitudIdAsync(solicitud.SolicitudId)!;
        solicitud.Id = solicitudActual.Id;
        solicitud.SolicitudPadreId = solicitudActual.SolicitudPadreId;
        return await _solicitudService.ActualizarSolicitudAsync(solicitud);
    }
}


