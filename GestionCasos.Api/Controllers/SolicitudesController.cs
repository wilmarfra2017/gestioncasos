using FluentValidation;
using GestionCasos.Api.Attributes;
using GestionCasos.Api.Dtos;
using GestionCasos.Api.Mappers;
using GestionCasos.Application.Dtos;
using GestionCasos.Application.Features.Adjuntos.Commands.AdjuntarDocumento;
using GestionCasos.Application.Features.Solicitudes.Commands.ActualizarSolicitud;
using GestionCasos.Application.Features.Solicitudes.Commands.CrearSolicitud;
using GestionCasos.Application.Features.Solicitudes.Queries.ObtenerSolicitudes;
using GestionCasos.Domain.Exceptions;
using GestionCasos.Infrastructure.Config;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GestionCasos.Api.Controllers;

[ApiControllerBase]
public class SolicitudesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly HttpConfig _httpConfig;
    private readonly IValidator<CrearSolicitudesCommand> _validatorCrearSolicitudesCommand;
    private readonly IValidator<CargarAdjuntoCommand> _validatorCargarAdjuntoCommand;
    private readonly IValidator<ObtenerSolicitudesQuery> _validatorObtenerSolicitudesQuery;
    private readonly IValidator<ActualizarSolicitudCommand> _validatorActualizarSolicitudCommand;
    public SolicitudesController(IMediator mediator,
                                HttpConfig httpConfig,
                                IValidator<CrearSolicitudesCommand> validatorCrearSolicitudesCommand,
                                IValidator<CargarAdjuntoCommand> validatorCargarAdjuntoCommand,
                                IValidator<ObtenerSolicitudesQuery> validatorObtenerSolicitudesQuery,
                                IValidator<ActualizarSolicitudCommand> validatorActualizarSolicitudCommand
                                )
    {
        _mediator = mediator;
        _httpConfig = httpConfig ?? throw new ArgumentNullException(nameof(httpConfig));
        _validatorCrearSolicitudesCommand = validatorCrearSolicitudesCommand ?? throw new ArgumentNullException(nameof(validatorCrearSolicitudesCommand));
        _validatorCargarAdjuntoCommand = validatorCargarAdjuntoCommand ?? throw new ArgumentNullException(nameof(validatorCargarAdjuntoCommand));
        _validatorObtenerSolicitudesQuery = validatorObtenerSolicitudesQuery ?? throw new ArgumentNullException(nameof(validatorObtenerSolicitudesQuery));
        _validatorActualizarSolicitudCommand = validatorActualizarSolicitudCommand ?? throw new ArgumentNullException(nameof(validatorActualizarSolicitudCommand));
    }

    [HttpPost]
    public async Task<ResponseCrearSolicitudDto> Crear([FromBody] SolicitudesRequestDto solicitudes)
    {
        string projectKey = Request.Headers[_httpConfig.ProyectoHeaderName]!;
        var comandoSolicitudes = CrearSolicitudesCommandMapper.ToCrearSolicitudesCommand(solicitudes, projectKey);
        var validaRequestSolicitudes = await _validatorCrearSolicitudesCommand.ValidateAsync(comandoSolicitudes);
        if (!validaRequestSolicitudes.IsValid)
        {
            throw new GestionCasosException(validaRequestSolicitudes.Errors.FirstOrDefault()!.ErrorMessage);
        }
        return await _mediator.Send(comandoSolicitudes);
    }

    [HttpGet]
    public async Task<ResponsePaginadoDto<ObtenerSolicitudesDto>> Obtener(
                                                        int pagina,
                                                        int tamanoPagina,
                                                        [FromQuery] string usuarioId,
                                                        [FromQuery] string? solicitudKey,
                                                        [FromQuery] string? estatusId,
                                                        [FromQuery] string? tipoSolicitudId
                                                        )

    {
        var query = new ObtenerSolicitudesQuery(
            pagina,
            tamanoPagina,
            usuarioId,
            solicitudKey,
            estatusId,
            tipoSolicitudId
        );
        var validaRequest = await _validatorObtenerSolicitudesQuery.ValidateAsync(query);

        if (!validaRequest.IsValid)
        {
            throw new GestionCasosException(validaRequest.Errors.FirstOrDefault()!.ErrorMessage);
        }

        return await _mediator.Send(query);
    }

    [HttpPost("{solicitudId}/{campoAdjuntoId}/adjuntos")]
    [ProducesResponseType(typeof(GenericResponseDto<AdjuntoDto>), (int)HttpStatusCode.OK)]
    public async Task<AdjuntoDto> CargarAdjuntoSolicitud([FromForm] FileDetailsDto fileDetails, [FromRoute] string solicitudId, [FromRoute] string campoAdjuntoId)
    {
        Stream? fileStream = fileDetails.File.OpenReadStream();

        CargarAdjuntoCommand command = new(
            StreamData: fileStream,
            Nombre: fileDetails.File.FileName,
            Formato: fileDetails.File.ContentType,
            PesoArchivo: fileDetails.File.Length,
            SolicitudId: solicitudId,
            CampoAdjuntoId: campoAdjuntoId,
            Descripcion: null
        );

        var validaRequest = await _validatorCargarAdjuntoCommand.ValidateAsync(command);
        if (!validaRequest.IsValid)
        {
            throw new GestionCasosException(validaRequest.Errors.FirstOrDefault()!.ErrorMessage);
        }

        return await _mediator.Send(command);
    }

    [HttpPatch("{solicitudId}")]
    public async Task Actualizar(string solicitudId, [FromBody] ActualizacionRequestDto request)
    {
        var comandoSolicitudes = new ActualizarSolicitudCommand(solicitudId, request.Campos);
        
        var validaRequest = await _validatorActualizarSolicitudCommand.ValidateAsync(comandoSolicitudes);

        if (!validaRequest.IsValid)
        {
            throw new GestionCasosException(validaRequest.Errors.FirstOrDefault()!.ErrorMessage);
        }
        
        await _mediator.Send(comandoSolicitudes);
    }
}

