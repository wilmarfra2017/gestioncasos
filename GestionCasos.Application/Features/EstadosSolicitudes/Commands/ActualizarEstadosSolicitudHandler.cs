using gestion_casos.Domain.Utils.GeneradorId;
using GestionCasos.Domain.Dtos;
using GestionCasos.Domain.Entities;
using GestionCasos.Domain.Exceptions;
using GestionCasos.Domain.Ports;
using GestionCasos.Domain.Services;
using MediatR;

namespace GestionCasos.Application.Features.EstadosSolicitudes.Commands;

public sealed class ActualizarEstadosSolicitudHandler(SolicitudService _solicitudService,
                                                        ISolicitudQueriesRepository _solicitudRepository,
                                                        ILogMessageService _resourceManager,
                                                        INotificacionApiEmailService _notificacionEmailService,
                                                        INotificacionApiSmsService _notificacionSmsService
                                                        ) : IRequestHandler<ActualizarEstadosSolicitudCommand, bool>

{
    async Task<bool> IRequestHandler<ActualizarEstadosSolicitudCommand, bool>.Handle(ActualizarEstadosSolicitudCommand request, CancellationToken cancellationToken)
    {
        return await ActualizarEstadoSolicitudEnBaseDeDatosAsync(request) is not null;
    }

    private async Task<Solicitud> ActualizarEstadoSolicitudEnBaseDeDatosAsync(ActualizarEstadosSolicitudCommand actualizarEstadosSolicitudCommand)
    {
        Solicitud solicitudActual = await _solicitudRepository.ObtenerPorSolicitudKeyAsync(actualizarEstadosSolicitudCommand.Key!);
        if (solicitudActual?.Id is null)
        {
            throw new GestionCasosException($"{_resourceManager.ErrorValidacionEstadoSolicitud} {solicitudActual?.Id}");
        }
        solicitudActual.Estatus = new Estatus(actualizarEstadosSolicitudCommand.Estatus!.Id, actualizarEstadosSolicitudCommand.Estatus!.Nombre);
        ConvertirFechaActualizacion(actualizarEstadosSolicitudCommand, solicitudActual, _resourceManager);
        solicitudActual.Comentario = actualizarEstadosSolicitudCommand.Comentarios?.Length > 0 ? actualizarEstadosSolicitudCommand.Comentarios[^1] : solicitudActual.Comentario;
        var solicitudActualizada = await _solicitudService.ActualizarSolicitudAsync(solicitudActual);
        var tareaNotificacionEmail = NotificacionEmailAsync(solicitudActualizada);
        var tareaNotificacionSms = NotificacionSmsAsync(solicitudActualizada);
        await Task.WhenAll(tareaNotificacionEmail, tareaNotificacionSms);
        return solicitudActualizada;
    }

    private static void ConvertirFechaActualizacion(ActualizarEstadosSolicitudCommand actualizarEstadosSolicitudCommand, Solicitud solicitudActual, ILogMessageService resourceManager)
    {
        if (FechaUtil.TryParseFechaExacta(actualizarEstadosSolicitudCommand.FechaActualizacion, out DateTime fechaConvertida))
        {
            solicitudActual.FechaModificacion = fechaConvertida;
        }
        else
        {
            throw new GestionCasosException(resourceManager.ErrorFormatoDeFechaNoValido);
        }
    }

    private async Task<bool> NotificacionEmailAsync(Solicitud solicitudActualizada)
    {
        NotificacionEmailRequestDto notificacionEmailRequest = new(
            "email@gmail.com",
            solicitudActualizada.Comentario!,
            new Estatus(solicitudActualizada.Estatus.Id, solicitudActualizada.Estatus.Nombre),
            "NombreIntermediario",
            solicitudActualizada.SolicitudKey);
        return await _notificacionEmailService.EnviarCorreoNotificacionAsync(notificacionEmailRequest);
    }

    private async Task<bool> NotificacionSmsAsync(Solicitud solicitudActualizada)
    {
        NotificacionSmsRequestDto notificacionSmsRequest = new(
            "+570000000000",
            solicitudActualizada.Comentario!,
            new Estatus(solicitudActualizada.Estatus.Id, solicitudActualizada.Estatus.Nombre),
            "NombreIntermediario",
            solicitudActualizada.SolicitudKey);
        return await _notificacionSmsService.EnviarSmsNotificacionAsync(notificacionSmsRequest);
    }
}
