using GestionCasos.Application.Features.EstadosSolicitudes.Commands;
using GestionCasos.Domain.Dtos;
using GestionCasos.Domain.Entities;

namespace GestionCasos.Api.Mappers;

public static class ActualizarEstadosSolicitudesMapper
{
    public static ActualizarEstadosSolicitudCommand ToCrearEstadosSolicitudesCommand(this WebHookSolicitudActualizadaRequestDto payload)
    {

        ArgumentNullException.ThrowIfNull(payload);

        return new ActualizarEstadosSolicitudCommand(
            payload.Key,
            payload.Fields.Updated,
            new Estatus(payload.Fields.Status.Id, payload.Fields.Status.Name),
            payload.Fields.Comment?.Comments);

    }
}
