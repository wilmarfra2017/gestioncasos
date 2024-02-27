using MediatR;

namespace GestionCasos.Application.Features.Solicitudes.Commands.ActualizarSolicitud;

public record ActualizarSolicitudCommand(
    string SolicitudId,
     Dictionary<string, object> Campos
 ) : IRequest<bool>;
