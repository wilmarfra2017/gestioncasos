using GestionCasos.Domain.Dtos;
using GestionCasos.Domain.Entities;
using GestionCasos.Domain.Ports;
using Microsoft.Extensions.Options;

namespace GestionCasos.Domain.Services;

[DomainService]
public class EstructuraSmsNotificacionService : IEstructuraSmsService
{
    private readonly ConfiguracionNotificacion _configuracionApiNotificacion;
    private readonly ILogMessageService _resourceManager;

    public EstructuraSmsNotificacionService(IOptions<ConfiguracionNotificacion> configuracionApiNotificacion,
                                ILogMessageService resourceManager)
    {
        _configuracionApiNotificacion = configuracionApiNotificacion.Value;
        _resourceManager = resourceManager;
    }

    public async Task<EstructuraSmsNotificacion> CrearEstructuraSmsAsync(string numero, string comentario, string estado, string nombreIntermediario, string SolicitudKey)
    {
        var metadatos = new List<MetadataDto>
        {
            new() { Key = _resourceManager.NombreIntermediario, description = _resourceManager.NombreDestinatario, isRequired = true, Value = nombreIntermediario },
            new() { Key = _resourceManager.NumeroSolicitud, description = _resourceManager.NumeroSolicitudJira, isRequired = true, Value = SolicitudKey },
            new() { Key = _resourceManager.EstatusSolicitud, description = _resourceManager.EstatusCambioSolicitud, isRequired = true, Value = estado },
            new() { Key = _resourceManager.Enlace, description = _resourceManager.EnlaceSolicitud, isRequired = true, Value = _configuracionApiNotificacion.DomainJira + SolicitudKey },
            new() { Key = _resourceManager.Comentario, description = _resourceManager.Comentario_Jira, isRequired = true, Value = comentario }
        };


        var estructuraSms = new EstructuraSmsNotificacion
        {
            NumeroDestinatario = numero,
            Plantilla = new Plantilla
            {
                Nombre = _configuracionApiNotificacion.NameTemplateSms,
                NombrePlataforma = _configuracionApiNotificacion.PlatformName,
                Idioma = _resourceManager.IdiomaPlantilla,
                Metadatos = metadatos
            },
            NombreProveedor = _configuracionApiNotificacion.SmsProvider
        };

        return await Task.FromResult(estructuraSms);
    }
}
