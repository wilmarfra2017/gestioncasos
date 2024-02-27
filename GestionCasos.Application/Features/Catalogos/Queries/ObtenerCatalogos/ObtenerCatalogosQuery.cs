using GestionCasos.Application.Dtos;
using MediatR;

namespace GestionCasos.Application.Features.Catalogos.Queries.ObtenerCatalogos;

public record ObtenerCatalogosQuery(
    int Pagina,
    int TamanoPagina,
    string? Nombre,
    string? ContieneClave,
    string? ContieneValor
) : IRequest<ResponsePaginadoDto<CatalogoDto>>;
