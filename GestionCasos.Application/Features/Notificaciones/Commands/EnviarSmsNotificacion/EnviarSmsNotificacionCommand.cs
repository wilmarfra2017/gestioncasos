using MediatR;

namespace GestionCasos.Application.Features.Notificaciones.Commands.EnviarSmsNotificacion;

public record EnviarSmsNotificacionCommand(
    string Numero,
    string Comentario,
    string Estado,
    string NombreIntermediario,
    string SolicitudKey
) : IRequest<bool>;
