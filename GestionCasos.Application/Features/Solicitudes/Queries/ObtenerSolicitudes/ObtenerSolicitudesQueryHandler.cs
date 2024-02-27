using GestionCasos.Domain.Ports;
using GestionCasos.Application.Dtos;
using GestionCasos.Application.Mappers;
using MediatR;

namespace GestionCasos.Application.Features.Solicitudes.Queries.ObtenerSolicitudes;


public class ObtenerSolicitudesQueryHandler(ISolicitudQueriesRepository _solicitudRepository) : IRequestHandler<ObtenerSolicitudesQuery, ResponsePaginadoDto<ObtenerSolicitudesDto>>
{
    public Task<ResponsePaginadoDto<ObtenerSolicitudesDto>> Handle(ObtenerSolicitudesQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return HandleAsync(request);
    }

    private async Task<ResponsePaginadoDto<ObtenerSolicitudesDto>> HandleAsync(ObtenerSolicitudesQuery request)
    {
        var solicitudes = await _solicitudRepository.ObtenerSolicitudesPaginadoAsync(request.Pagina,
                                                        request.TamanoPagina,
                                                        request.UsuarioId,
                                                        request.SolicitudKey,
                                                        request.EstatusId,
                                                        request.TipoSolicitudId
                                                        );
        var solicitudesDto = SolicitudMapper.ToResponsePaginadoDto(solicitudes, request.Pagina, request.TamanoPagina);
        return solicitudesDto;
    }
}




