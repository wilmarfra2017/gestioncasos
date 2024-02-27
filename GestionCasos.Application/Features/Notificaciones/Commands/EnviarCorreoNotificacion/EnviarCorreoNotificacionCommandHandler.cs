using gestion_casos.Domain.Exceptions;
using GestionCasos.Domain.Ports;
using MediatR;

namespace GestionCasos.Application.Features.Notificaciones.Commands.EnviarCorreoNotificacion;

public class EnviarCorreoNotificacionCommandHandler(IEstructuraCorreoService _estructuraService, INotificacionEmailService _emailService, IPlantillaService _plantillaService,
    ILogMessageService _resourceManager) : IRequestHandler<EnviarCorreoNotificacionCommand, bool>
{
    public async Task<bool> Handle(EnviarCorreoNotificacionCommand request, CancellationToken cancellationToken)
    {
        var estructuraNotificacion = await _estructuraService.CrearEstructuraCorreoAsync(
           request.Email,
           request.Comentario,
           request.Estado,
           request.NombreIntermediario,
           request.SolicitudKey);

        var templateResponse = await _plantillaService.ObtenerPlantillaAsync(estructuraNotificacion.Plantilla.Nombre, estructuraNotificacion.Plantilla.NombrePlataforma);

        if (!templateResponse.Success)
        {
            throw new EmailNotificacionException(_resourceManager.PlantillaNoExiste);
        }
        return await _emailService.EnviarCorreoNotificacionAsync(estructuraNotificacion);
    }
}
