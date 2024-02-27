using GestionCasos.Domain.Entities;

namespace GestionCasos.Domain.Tests.DataBuilder;

public class ConfiguracionNotificacionCorreoBuilder
{
    private readonly ConfiguracionNotificacion _configuracionNotificacion = new();

    public ConfiguracionNotificacionCorreoBuilder ConDominioJira(string dominio)
    {
        _configuracionNotificacion.DomainJira = dominio;
        return this;
    }

    public ConfiguracionNotificacionCorreoBuilder ConNombrePlantillaEmail(string nombrePlantilla)
    {
        _configuracionNotificacion.NameTemplateEmail = nombrePlantilla;
        return this;
    }

    public ConfiguracionNotificacionCorreoBuilder ConNombrePlataforma(string nombrePlataforma)
    {
        _configuracionNotificacion.PlatformName = nombrePlataforma;
        return this;
    }

    public ConfiguracionNotificacionCorreoBuilder ConProveedorInterno(string proveedorInterno)
    {
        _configuracionNotificacion.InternalProvider = proveedorInterno;
        return this;
    }

    public ConfiguracionNotificacionCorreoBuilder ConProveedorExterno(string proveedorExterno)
    {
        _configuracionNotificacion.ExternalProvider = proveedorExterno;
        return this;
    }

    public ConfiguracionNotificacion Build()
    {
        return _configuracionNotificacion;
    }
}