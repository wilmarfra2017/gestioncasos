using FluentValidation.Results;
using gestion_casos.Api.Validators;
using GestionCasos.Api.Attributes;
using GestionCasos.Application.Features.Notificaciones.Commands.EnviarSmsNotificacion;
using GestionCasos.Domain.Dtos;
using GestionCasos.Domain.Ports;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace gestion_casos.Api.Controllers;

[ApiControllerBase]
public class NotificacionesSmsController(IMediator mediator, ILogMessageService resourceManager) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogMessageService _resourceManager = resourceManager;

    [HttpPost]
    public async Task<IActionResult> EnviarSmsNotificacion([FromBody] NotificacionSmsRequestDto notificacion)
    {
        NotificacionSmsRequestDtoValidator validator = new NotificacionSmsRequestDtoValidator();
        ValidationResult validationResult = validator.Validate(notificacion);

        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }

        var command = new EnviarSmsNotificacionCommand(
            notificacion.Numero,
            notificacion.Comentario,
            notificacion.Estatus.Nombre,
            notificacion.NombreIntermediario,
            notificacion.SolicitudKey);

        var respuesta = await _mediator.Send(command);

        if (!respuesta)
        {
            return BadRequest($"{_resourceManager.ErrorNotificacionSms}");
        }

        return Ok();
    }
}
