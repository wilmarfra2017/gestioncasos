using GestionCasos.Domain.Dtos;
using GestionCasos.Domain.Entities;
using GestionCasos.Domain.Ports;
using GestionCasos.Domain.Exceptions;
using GestionCasos.Infrastructure.Helpers;
using GestionCasos.Infrastructure.Ports;
using MongoDB.Driver;

namespace GestionCasos.Infrastructure.Adapters;

[Repository]
public class CatalogoRepository : ICatalogoRepository
{
    private readonly IRepository<Catalogo> _catalogoRepository;
    private readonly ILogMessageService _resourceManager;

    public CatalogoRepository(IRepository<Catalogo> catalogoRepository, ILogMessageService resourceManager)
    {
        _catalogoRepository = catalogoRepository ?? throw new ArgumentNullException(nameof(catalogoRepository));
        _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
    }
    public async Task<PaginadoDto<Catalogo>> ObtenerCatalogosPaginadoAsync(
                                                                                int pagina,
                                                                                int tamanoPagina,
                                                                                string? nombreCatalogo,
                                                                                string? conClave,
                                                                                string? conValor
                                                                                )
    {
        try
        {
            var filter = ConstruirFiltroParaCatalogo(nombreCatalogo, conClave, conValor);
            var skip = PaginadoHelper.CalcularSkip(pagina, tamanoPagina);
            var catalogos = await _catalogoRepository.GetManyByFilterPaginatedAsync(filter, skip, tamanoPagina);
            var totalRegistros = await _catalogoRepository.GetTotalRecordsByFilterAsync(filter);
            var totalPaginas = PaginadoHelper.CalcularTotalPaginas(totalRegistros, tamanoPagina);
            var response = new PaginadoDto<Catalogo>
            {
                Data = catalogos,
                TotalRegistros = totalRegistros,
                TotalPaginas = totalPaginas
            };
            return response;
        }
        catch (System.Exception ex)
        {
            throw new PersistenciaException($"{_resourceManager.ErrorPersistencia} {ex.Message}");
        }
    }

    private static FilterDefinition<Catalogo> ConstruirFiltroParaCatalogo(string? nombreCatalogo, string? conClave, string? conValor)
    {
        var builder = Builders<Catalogo>.Filter;

        var filters = new List<FilterDefinition<Catalogo>>();

        if (!string.IsNullOrWhiteSpace(nombreCatalogo))
        {
            var nombreFilter = builder.Eq(model => model.Nombre, nombreCatalogo);
            filters.Add(nombreFilter);
        }

        if (!string.IsNullOrWhiteSpace(conClave))
        {
            var conClaveFilter = builder.ElemMatch(model => model.Elementos, model => model.Clave == conClave);
            filters.Add(conClaveFilter);
        }

        if (!string.IsNullOrWhiteSpace(conValor))
        {
            var conValorFilter = builder.ElemMatch(model => model.Elementos, model => model.Valor == conValor);
            filters.Add(conValorFilter);
        }

        return filters.Count > 1 ? builder.And(filters) : filters[0];
    }
}
