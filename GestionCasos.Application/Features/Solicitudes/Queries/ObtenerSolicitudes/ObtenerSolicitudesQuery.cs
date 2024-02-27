using GestionCasos.Application.Dtos;
using MediatR;

namespace GestionCasos.Application.Features.Solicitudes.Queries.ObtenerSolicitudes;

public record ObtenerSolicitudesQuery(
    int Pagina,
    int TamanoPagina,
    string UsuarioId,
    string? SolicitudKey,
    string? EstatusId,
    string? TipoSolicitudId
) : IRequest<ResponsePaginadoDto<ObtenerSolicitudesDto>>;

