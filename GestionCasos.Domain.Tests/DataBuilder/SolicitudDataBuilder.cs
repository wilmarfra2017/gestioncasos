
using GestionCasos.Domain.Entities;

namespace GestionCasos.Domain.Tests.DataBuilder;
public class SolicitudDataBuilder
{
    private readonly Solicitud _solicitud = new();

    public SolicitudDataBuilder WithProyectoId(string proyectoId)
    {
        _solicitud.ProyectoId = proyectoId;
        return this;
    }

    public SolicitudDataBuilder WithSolicitudId(string solicitudId)
    {
        _solicitud.SolicitudId = solicitudId;
        return this;
    }
    public SolicitudDataBuilder WithSolicitudKey(string solicitudKey)
    {
        _solicitud.SolicitudKey = solicitudKey;
        return this;
    }
    public SolicitudDataBuilder WithSolicitudPadreId(string solicitudPadreId)
    {
        _solicitud.SolicitudPadreId = solicitudPadreId;
        return this;
    }

    public SolicitudDataBuilder WithProyectoKey(string proyectoKey)
    {
        _solicitud.ProyectoKey = proyectoKey;
        return this;
    }
    public SolicitudDataBuilder WithResumen(string resumen)
    {
        _solicitud.Resumen = resumen;
        return this;
    }

    public SolicitudDataBuilder WithTipoSolicitud(TipoSolicitud tipoSolicitud)
    {
        _solicitud.TipoSolicitud = tipoSolicitud;
        return this;
    }

    public SolicitudDataBuilder WithOpcionalTitulo(string titulo)
    {
        _solicitud.Titulo = titulo;
        return this;
    }

    public SolicitudDataBuilder WithOpcionalDescripcion(string descripcion)
    {
        _solicitud.Descripcion = descripcion;
        return this;
    }

    public SolicitudDataBuilder WithOpcionalUsuarioAsignado(Usuario? UsuarioAsignado)
    {
        _solicitud.UsuarioAsignado = UsuarioAsignado;
        return this;
    }

    public SolicitudDataBuilder WithOpcionalUsuarioCreacion(Usuario? UsuarioCreacion)
    {
        _solicitud.UsuarioCreacion = UsuarioCreacion;
        return this;
    }

    public SolicitudDataBuilder WithOpcionalUsuarioModificacion(Usuario? UsuarioModificacion)
    {
        _solicitud.UsuarioModificacion = UsuarioModificacion;
        return this;
    }

    public SolicitudDataBuilder WithOpcionalAdjunto(Adjunto? adjunto)
    {
        _solicitud.Adjunto = adjunto;
        return this;
    }

    public Solicitud Build()
    {
        return _solicitud;
    }
}
