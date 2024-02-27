using GestionCasos.Domain.Dtos;
using GestionCasos.Domain.Entities;
using GestionCasos.Domain.Ports;
using Microsoft.Extensions.Options;

namespace GestionCasos.Domain.Services;

[DomainService]
public class EstructuraCorreoNotificacionService : IEstructuraCorreoService
{
    private readonly ConfiguracionNotificacion _configuracionApiNotificacion;
    private readonly DominioHumano _dominioHumano;
    private readonly ILogMessageService _resourceManager;

    public EstructuraCorreoNotificacionService(IOptions<ConfiguracionNotificacion> configuracionApiNotificacion, DominioHumano dominioHumano,
                                        ILogMessageService resourceManager)
    {
        _configuracionApiNotificacion = configuracionApiNotificacion.Value;
        _dominioHumano = dominioHumano;
        _resourceManager = resourceManager;
    }

    public async Task<EstructuraCorreoNotificacion> CrearEstructuraCorreoAsync(string email, string comentario, string estado, string nombreIntermediario, string SolicitudKey)
    {
        var metadatos = new List<MetadataDto>
        {
            new() { Key = _resourceManager.NombreIntermediario, description = _resourceManager.NombreDestinatario, isRequired = true, Value = nombreIntermediario },
            new() { Key = _resourceManager.NumeroSolicitud, description = _resourceManager.NumeroSolicitudJira, isRequired = true, Value = SolicitudKey },
            new() { Key = _resourceManager.EstatusSolicitud, description = _resourceManager.EstatusCambioSolicitud, isRequired = true, Value = estado },
            new() { Key = _resourceManager.Enlace, description = _resourceManager.EnlaceSolicitud, isRequired = true, Value = _configuracionApiNotificacion.DomainJira + SolicitudKey },
            new() { Key = _resourceManager.Comentario, description = _resourceManager.Comentario_Jira, isRequired = true, Value = comentario }
        };

        var estructura = new EstructuraCorreoNotificacion
        {
            CorreoDestinatario = email,
            Plantilla = new Plantilla
            {
                Nombre = _configuracionApiNotificacion.NameTemplateEmail,
                NombrePlataforma = _configuracionApiNotificacion.PlatformName,
                Idioma = _resourceManager.IdiomaPlantilla,
                Metadatos = metadatos
            },
            NombreProveedor = email.Contains(_dominioHumano.InternalMailDomain, StringComparison.InvariantCultureIgnoreCase)
                              ? _configuracionApiNotificacion.InternalProvider
                              : _configuracionApiNotificacion.ExternalProvider
        };

        return await Task.FromResult(estructura);
    }
}
