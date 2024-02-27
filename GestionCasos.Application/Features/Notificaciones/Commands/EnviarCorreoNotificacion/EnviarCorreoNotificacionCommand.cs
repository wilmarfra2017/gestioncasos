using MediatR;

namespace GestionCasos.Application.Features.Notificaciones.Commands.EnviarCorreoNotificacion;

public record EnviarCorreoNotificacionCommand(
    string Email,
    string Comentario,
    string Estado,
    string NombreIntermediario,
    string SolicitudKey
) : IRequest<bool>;
