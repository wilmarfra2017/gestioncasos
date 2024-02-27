using GestionCasos.Domain.Ports;
using GestionCasos.Application.Dtos;
using GestionCasos.Application.Mappers;
using MediatR;

namespace GestionCasos.Application.Features.Catalogos.Queries.ObtenerCatalogos;

public class ObtenerCatalogosQueryHandler(ICatalogoRepository _catalogoRepository) : IRequestHandler<ObtenerCatalogosQuery, ResponsePaginadoDto<CatalogoDto>>
{
    public Task<ResponsePaginadoDto<CatalogoDto>> Handle(ObtenerCatalogosQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return HandleAsync(request);
    }

    private async Task<ResponsePaginadoDto<CatalogoDto>> HandleAsync(ObtenerCatalogosQuery request)
    {
        var catalogoPaginado = await _catalogoRepository.ObtenerCatalogosPaginadoAsync(request.Pagina, request.TamanoPagina, request.Nombre, request.ContieneClave, request.ContieneValor);
        var response = CatalogoMapper.ToResponsePaginadoDto(catalogoPaginado, request.Pagina, request.TamanoPagina);
        return response;
    }
}


