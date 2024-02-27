using FluentValidation;
using GestionCasos.Api.Attributes;
using GestionCasos.Application.Dtos;
using GestionCasos.Application.Features.Catalogos.Queries.ObtenerCatalogos;
using GestionCasos.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionCasos.Api.Controllers;

[ApiControllerBase]
public class CatalogosController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<ObtenerCatalogosQuery> _validatorObtenerCatalogosQuery;
    public CatalogosController(IMediator mediator,
                               IValidator<ObtenerCatalogosQuery> validatorObtenerCatalogosQuery
                                )
    {
        _mediator = mediator;
        _validatorObtenerCatalogosQuery = validatorObtenerCatalogosQuery ?? throw new ArgumentNullException(nameof(validatorObtenerCatalogosQuery));
    }

    [HttpGet]
    public async Task<ResponsePaginadoDto<CatalogoDto>> ObtenerCatalogos(
                                                        int pagina,
                                                        int tamanoPagina,
                                                        [FromQuery] string? nombre,
                                                        [FromQuery] string? contieneClave,
                                                        [FromQuery] string? contieneValor
                                                        )

    {
        var query = new ObtenerCatalogosQuery(
            pagina,
            tamanoPagina,
            nombre,
            contieneClave,
            contieneValor
        );
        var validaRequest = await _validatorObtenerCatalogosQuery.ValidateAsync(query);
        if (!validaRequest.IsValid)
        {
            throw new GestionCasosException(validaRequest.Errors.FirstOrDefault()!.ErrorMessage);
        }
        return await _mediator.Send(query);
    }
}
