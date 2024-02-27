using GestionCasos.Domain.Dtos;
using GestionCasos.Domain.Entities;

namespace GestionCasos.Domain.Ports;
public interface ISolicitudQueriesRepository
{
    Task<PaginadoDto<Solicitud>> ObtenerSolicitudesPaginadoAsync(
                                                                  int pagina,
                                                                  int tamanoPagina,
                                                                  string usuarioId,
                                                                  string? solicitudKey,
                                                                  string? estatusId,
                                                                  string? tipoSolicitudId
                                                                );

    Task<Solicitud> ObtenerPorSolicitudIdAsync(string solicitudId);
    Task<Solicitud> ObtenerPorSolicitudKeyAsync(string solicitudkey);
}


