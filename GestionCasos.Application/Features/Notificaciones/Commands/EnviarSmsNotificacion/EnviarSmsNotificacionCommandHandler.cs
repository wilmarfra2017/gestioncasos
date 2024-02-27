using gestion_casos.Domain.Exceptions;
using GestionCasos.Domain.Ports;
using MediatR;

namespace GestionCasos.Application.Features.Notificaciones.Commands.EnviarSmsNotificacion;

public class EnviarSmsNotificacionCommandHandler(IEstructuraSmsService _estructuraService, INotificacionSmsService _smsService,
    IPlantillaService _plantillaService, ILogMessageService _resourceManager) : IRequestHandler<EnviarSmsNotificacionCommand, bool>
{
    public async Task<bool> Handle(EnviarSmsNotificacionCommand request, CancellationToken cancellationToken)
    {
        var estructuraNotificacion = await _estructuraService.CrearEstructuraSmsAsync(
           request.Numero,
           request.Comentario,
           request.Estado,
           request.NombreIntermediario,
           request.SolicitudKey);

        //var templateResponse = await _plantillaService.ObtenerPlantillaAsync(estructuraNotificacion.Plantilla.Nombre, estructuraNotificacion.Plantilla.NombrePlataforma);

        //if (!templateResponse.Success)
        //{
        //    throw new SmsNotificacionException(_resourceManager.PlantillaNoExiste);
        //}

        return await _smsService.EnviarSmsNotificacionAsync(estructuraNotificacion);
    }
}