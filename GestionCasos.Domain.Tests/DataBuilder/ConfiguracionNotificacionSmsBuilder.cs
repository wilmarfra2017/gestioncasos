using GestionCasos.Domain.Entities;

namespace gestion_casos.Domain.Tests.DataBuilder;

public class ConfiguracionNotificacionSmsBuilder
{

    private readonly ConfiguracionNotificacion _configuracionNotificacion = new();

    public ConfiguracionNotificacionSmsBuilder ConDominioJira(string dominio)
    {
        _configuracionNotificacion.DomainJira = dominio;
        return this;
    }

    public ConfiguracionNotificacionSmsBuilder ConNombrePlantillaSms(string nombrePlantilla)
    {
        _configuracionNotificacion.NameTemplateSms = nombrePlantilla;
        return this;
    }

    public ConfiguracionNotificacionSmsBuilder ConNombrePlataforma(string nombrePlataforma)
    {
        _configuracionNotificacion.PlatformName = nombrePlataforma;
        return this;
    }

    public ConfiguracionNotificacionSmsBuilder ConNombreProveedor(string nombreProveedor)
    {
        _configuracionNotificacion.SmsProvider = nombreProveedor;
        return this;
    }

    public ConfiguracionNotificacion Build()
    {
        return _configuracionNotificacion;
    }
}