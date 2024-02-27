using GestionCasos.Domain.Dtos;
using GestionCasos.Domain.Entities;
using GestionCasos.Domain.Ports;
using GestionCasos.Domain.Exceptions;
using GestionCasos.Infrastructure.Helpers;
using GestionCasos.Infrastructure.Ports;
using MongoDB.Driver;

namespace GestionCasos.Infrastructure.Adapters
{
    [Repository]
    public class SolicitudQueriesRepository(IRepository<Solicitud> solicitudRepository, ILogMessageService _resourceManager) : ISolicitudQueriesRepository
    {
        readonly IRepository<Solicitud> _solicitudRepository = solicitudRepository
            ?? throw new ArgumentNullException(nameof(solicitudRepository));

        public async Task<PaginadoDto<Solicitud>> ObtenerSolicitudesPaginadoAsync(
                                                                int pagina,
                                                                int tamanoPagina,
                                                                string usuarioId,
                                                                string? solicitudKey,
                                                                string? estatusId,
                                                                string? tipoSolicitudId
                                                              )
        {
            try
            {
                var filter = ConstruirFiltroParaCatalogo(usuarioId, solicitudKey, estatusId, tipoSolicitudId);
                var skip = PaginadoHelper.CalcularSkip(pagina, tamanoPagina);
                var solicitudes = await _solicitudRepository.GetManyByFilterPaginatedAsync(filter, skip, tamanoPagina);
                var totalRegistros = await _solicitudRepository.GetTotalRecordsByFilterAsync(filter);
                var totalPaginas = PaginadoHelper.CalcularTotalPaginas(totalRegistros, tamanoPagina);
                var response = new PaginadoDto<Solicitud>
                {
                    Data = solicitudes,
                    TotalRegistros = totalRegistros,
                    TotalPaginas = totalPaginas
                };
                return response;
            }
            catch (System.Exception ex)
            {
                throw new PersistenciaException($"{_resourceManager.ErrorValidacionBaseDeDatos} {ex.Message}");
            }
        }
        public async Task<Solicitud> ObtenerPorSolicitudIdAsync(
                                                        string solicitudId
                                                        )
        {
            try
            {
                var builder = Builders<Solicitud>.Filter;
                var filter = builder.Eq(model => model.SolicitudId, solicitudId);
                var solicitud = await _solicitudRepository.GetOneByFilterAsync(filter);
                return solicitud;
            }
            catch (System.Exception ex)
            {
                throw new PersistenciaException($"{_resourceManager.ErrorValidacionBaseDeDatos} {ex.Message}");
            }
        }

        public async Task<Solicitud> ObtenerPorSolicitudKeyAsync(
                                                        string solicitudkey
                                                        )
        {
            try
            {
                var builder = Builders<Solicitud>.Filter;
                var filter = builder.Eq(model => model.SolicitudKey, solicitudkey);
                var solicitud = await _solicitudRepository.GetOneByFilterAsync(filter);
                return solicitud;
            }
            catch (System.Exception ex)
            {
                throw new PersistenciaException($"{_resourceManager.ErrorValidacionBaseDeDatos} {ex.Message}");
            }
        }

        private static FilterDefinition<Solicitud> ConstruirFiltroParaCatalogo(string? usuarioId, string? solicitudKey, string? estatusId, string? tipoSolicitudId)
        {
            var builder = Builders<Solicitud>.Filter;

            var filters = new List<FilterDefinition<Solicitud>>();

            if (!string.IsNullOrWhiteSpace(usuarioId))
            {
                var nombreFilter = builder.Eq(model => model.UsuarioCreacion!.Id, usuarioId);
                filters.Add(nombreFilter);
            }
            if (!string.IsNullOrWhiteSpace(solicitudKey))
            {
                var nombreFilter = builder.Eq(model => model.SolicitudKey, solicitudKey);
                filters.Add(nombreFilter);
            }
            if (!string.IsNullOrWhiteSpace(estatusId))
            {
                var nombreFilter = builder.Eq(model => model.Estatus.Id, estatusId);
                filters.Add(nombreFilter);
            }
            if (!string.IsNullOrWhiteSpace(tipoSolicitudId))
            {
                var nombreFilter = builder.Eq(model => model.TipoSolicitud.Id, tipoSolicitudId);
                filters.Add(nombreFilter);
            }

            return filters.Count > 1 ? builder.And(filters) : filters[0];
        }
    }
}


