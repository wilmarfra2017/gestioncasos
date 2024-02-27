using GestionCasos.Domain.Dtos;
using GestionCasos.Domain.Ports;
using GestionCasos.Domain.Services;
using GestionCasos.Application.Dtos;
using GestionCasos.Application.Mappers;
using MediatR;

namespace GestionCasos.Application.Features.Solicitudes.Commands.CrearSolicitud;
public class CrearSolicitudesCommandHandler(
                                            SolicitudService _solicitudService,
                                            IReporteSolicitudService _reporteSolicitudService,
                                            GeneradorSolicitudIdService _generadorSolicitudId
                                        ) : IRequestHandler<CrearSolicitudesCommand, ResponseCrearSolicitudDto>
{
    public Task<ResponseCrearSolicitudDto> Handle(CrearSolicitudesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return HandleAsync(request);
    }
    private async Task<ResponseCrearSolicitudDto> HandleAsync(CrearSolicitudesCommand request)
    {
        var solicitudId = _generadorSolicitudId.GenerarId(request.PrefijoSolicitud);
        var solicitudesDto = await CrearYSincronizarSolicitudesAsync(request, solicitudId);
        return solicitudesDto;
    }

    private async Task<ResponseCrearSolicitudDto> CrearYSincronizarSolicitudesAsync(CrearSolicitudesCommand request, string solicitudId)
    {
        var nombreCampoSolicitudPadre = _solicitudService.ObtenerNombreSolicitudPadreId();
        var solicitudes = SolicitudMapper.ToRequestSolicitudDtos(request, nombreCampoSolicitudPadre, solicitudId).ToList();
        var respuestaCreacion = await _solicitudService.CrearSolicitudesAsync(solicitudes);
        await SincronizarSolicitudesConPadreAsync(respuestaCreacion.Issues, solicitudId);
        respuestaCreacion.SolicitudIdExterna = solicitudId;
        return SolicitudMapper.ToResponseCrearSolicitudDto(respuestaCreacion);
    }

    private async Task SincronizarSolicitudesConPadreAsync(IEnumerable<ResponseNuevaSolicitudDto> solicitudesCreadas, string solicitudPadreId)
    {
        var tareas = new List<Task>();
        foreach (var solicitud in solicitudesCreadas)
        {
            tareas.Add(VincularSolicitudConPadreAsync(solicitud.Id, solicitudPadreId));
        }
        await Task.WhenAll(tareas);
    }

    private async Task VincularSolicitudConPadreAsync(string idorkey, string solicitudPadreId)
    {
        var solicitudInfo = await _reporteSolicitudService.ConsultarAsync(idorkey)!;
        if (solicitudInfo is not null)
        {
            solicitudInfo.SolicitudPadreId = solicitudPadreId;
            await _solicitudService.GuardarSolicitudAsync(solicitudInfo);
        }
    }
}



