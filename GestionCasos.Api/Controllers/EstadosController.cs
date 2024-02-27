using FluentValidation;
using GestionCasos.Api.Attributes;
using GestionCasos.Api.Mappers;
using GestionCasos.Application.Features.EstadosSolicitudes.Commands;
using GestionCasos.Domain.Dtos;
using GestionCasos.Domain.Exceptions;
using GestionCasos.Domain.Ports;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionCasos.Api.Controllers;

[ApiControllerBase]
public class EstadosController(IMediator mediator,
                                    IValidator<ActualizarEstadosSolicitudCommand> validatorCrearSolicitudesCommand) : ControllerBase
{

    [HttpPost]
    public async Task<ActionResult> ActualizarEstadosSolicitudes([FromBody] WebHookSolicitudActualizadaRequestDto payload)
    {
        ActualizarEstadosSolicitudCommand comandoEstadosSolicitudes = ActualizarEstadosSolicitudesMapper.ToCrearEstadosSolicitudesCommand(payload);
        var validaRequestSolicitudes = await validatorCrearSolicitudesCommand.ValidateAsync(comandoEstadosSolicitudes);
        if (!validaRequestSolicitudes.IsValid)
        {
            throw new GestionCasosException(validaRequestSolicitudes.Errors.FirstOrDefault()!.ErrorMessage);
        }
        try
        {
            var resultado = await mediator.Send(comandoEstadosSolicitudes);
            return Ok(resultado);
        }
        catch (GestionCasosException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

