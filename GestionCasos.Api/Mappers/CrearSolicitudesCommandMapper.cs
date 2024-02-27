using GestionCasos.Api.Dtos;
using GestionCasos.Application.Features.Solicitudes.Commands.CrearSolicitud;
using GestionCasos.Domain.Exceptions;

namespace GestionCasos.Api.Mappers;
public static class CrearSolicitudesCommandMapper
{
    public static CrearSolicitudesCommand ToCrearSolicitudesCommand(this SolicitudesRequestDto solicitudesRequest, string projectKey)
    {
        ValidarRequest(solicitudesRequest);

        var solicitudesCommands = solicitudesRequest.Solicitudes
                                                    .Select(solicitudDto => solicitudDto.ToNuevaSolicitudCommand(projectKey))
                                                    .ToList();
        return new CrearSolicitudesCommand(solicitudesRequest.PrefijoSolicitud, solicitudesCommands);
    }

    private static NuevaSolicitud ToNuevaSolicitudCommand(this SolicitudRequestDto solicitudDto, string projectKey)
    {
        var tipoSolicitud = new TipoSolicitud(solicitudDto.TipoSolicitud.Id, solicitudDto.TipoSolicitud.Nombre);
        return new NuevaSolicitud(
            solicitudDto.Resumen,
            tipoSolicitud,
            projectKey,
            solicitudDto.UsuarioId,
            solicitudDto.Campos,
            solicitudDto.Descripcion
        );
    }

    private static void ValidarRequest(SolicitudesRequestDto solicitudesRequest)
    {
        _ = solicitudesRequest.Solicitudes?.Count > 0 ? true : throw new GestionCasosException("La lista de solicitudes no puede ser null o estar vacía.");

        if (!solicitudesRequest.Solicitudes.TrueForAll(solicitud => solicitud is not null))
        {
            throw new GestionCasosException("Una de las solicitudes es null.");
        }
    }
}
