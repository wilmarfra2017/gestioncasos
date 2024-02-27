using FluentValidation.Results;
using GestionCasos.Api.Attributes;
using GestionCasos.Application.Features.Notificaciones.Commands.EnviarCorreoNotificacion;
using GestionCasos.Domain.Dtos;
using GestionCasos.Domain.Ports;
using GestionCasos.Domain.Validators;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace gestion_casos.Api.Controllers;

[ApiControllerBase]
public class NotificacionesEmailController(IMediator _mediator, ILogMessageService _resourceManager) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> EnviarEmailApiNotificacion([FromBody] NotificacionEmailRequestDto notificacion)
    {
        NotificacionEmailRequestDtoValidator validator = new();
        ValidationResult validationResult = validator.Validate(notificacion);

        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }

        var command = new EnviarCorreoNotificacionCommand(
            notificacion.Email,
            notificacion.Comentario,
            notificacion.Estatus.Nombre,
            notificacion.NombreIntermediario,
            notificacion.SolicitudKey);

        var respuesta = await _mediator.Send(command);

        if (!respuesta)
        {
            return BadRequest($"{_resourceManager.ErrorNotificacionCorreo}");
        }
        return Ok();
    }
}
