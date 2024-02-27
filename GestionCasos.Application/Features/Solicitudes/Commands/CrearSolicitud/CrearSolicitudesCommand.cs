using GestionCasos.Application.Dtos;
using MediatR;


namespace GestionCasos.Application.Features.Solicitudes.Commands.CrearSolicitud;

public record CrearSolicitudesCommand(
    string PrefijoSolicitud,
    List<NuevaSolicitud> Solicitudes
 ) : IRequest<ResponseCrearSolicitudDto>;

public record NuevaSolicitud(
        string Resumen,
        TipoSolicitud TipoSolicitud,
        string ProjectKey,
        string UsuarioId,
        Dictionary<string, object> Campos,
        string? Descripcion
    );

public record TipoSolicitud(string Id, string Nombre);
public record Campo(string Id, string Valor);
