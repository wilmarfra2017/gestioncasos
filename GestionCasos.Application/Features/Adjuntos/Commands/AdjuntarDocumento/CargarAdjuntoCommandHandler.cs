using GestionCasos.Domain.Entities;
using GestionCasos.Domain.Services;
using GestionCasos.Application.Dtos;
using MediatR;
using GestionCasos.Domain.Ports;
using GestionCasos.Domain.Exceptions;

namespace GestionCasos.Application.Features.Adjuntos.Commands.AdjuntarDocumento;

public class CargarAdjuntoCommandHandler(AdjuntoService _adjuntoService,
                          ISolicitudQueriesRepository _solicitudRepository,
                          SolicitudService _solicitudService,
                          ActualizarSolicitudService _actualizarSolicitudService
                          ) : IRequestHandler<CargarAdjuntoCommand, AdjuntoDto>
{
  public Task<AdjuntoDto> Handle(CargarAdjuntoCommand request, CancellationToken cancellationToken)
  {
    ArgumentNullException.ThrowIfNull(request);
    return EnviarAStorageYActualizarAsync(request);
  }
  private async Task<AdjuntoDto> EnviarAStorageYActualizarAsync(CargarAdjuntoCommand request)
  {
    var solicitud = await ObtenerYValidarSolicitudAsync(request.SolicitudId);
    var adjunto = await EnviarAStorageAsync(request);
    var solicitudGuardada  = await ActualizarUrlEnBaseDeDatosAsync(solicitud, adjunto);
    await ActualizarEnJiraAsync(request.SolicitudId,solicitudGuardada.Adjunto!.Url!,request.CampoAdjuntoId);
    return adjunto;
  }

  private async Task ActualizarEnJiraAsync(string solicitudId, Uri adjuntoUrl, string campoCustomId)
  {
    var campoActualizar = _actualizarSolicitudService.BuildCampoCustom(campoCustomId,adjuntoUrl);
    await _actualizarSolicitudService.ActualizarSolicitudAsync(campoActualizar,solicitudId);
  }
  private async Task<AdjuntoDto> EnviarAStorageAsync(CargarAdjuntoCommand request)
  {
    var adjunto = await _adjuntoService.CargarAdjuntoAsync(request.Nombre, request.StreamData, request.Formato, request.SolicitudId);
    return new AdjuntoDto(adjunto.Url!, request.Nombre);
  }
  private async Task<Solicitud> ActualizarUrlEnBaseDeDatosAsync(Solicitud solicitud, AdjuntoDto adjunto)
  {
    var urlAdjunto = ObtenerUrlAdjunto(adjunto.UrlAdjunto);
    solicitud.Adjunto = new Adjunto { Url = urlAdjunto };
    return await _solicitudService.ActualizarSolicitudAsync(solicitud);
  }
  private async Task<Solicitud> ObtenerYValidarSolicitudAsync(string solicitudId)
  {
    var solicitud = await _solicitudRepository.ObtenerPorSolicitudIdAsync(solicitudId)!;
    if (solicitud is null || solicitud.Id is null)
    {
      throw new GestionCasosException($"No existe la solicitud");
    }
    return solicitud;
  }
  private static Uri ObtenerUrlAdjunto(Uri pathAdjunto)
  {
    string[] segments = pathAdjunto.Segments;
    string urlSinNombreArchivo = pathAdjunto.GetLeftPart(UriPartial.Authority) + string.Join("", segments, 0, segments.Length - 1).TrimEnd('/');
    return new Uri(urlSinNombreArchivo);
  }
}





