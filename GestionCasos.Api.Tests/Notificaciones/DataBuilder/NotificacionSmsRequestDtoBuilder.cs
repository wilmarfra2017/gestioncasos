using GestionCasos.Domain.Entities;

namespace GestionCasos.Api.Tests.Notificaciones.DataBuilder;

public class NotificacionSmsRequestDtoBuilder
{
    private string numero = "573227008360";
    private string comentario = "Te falta foto titular en ticket JRASERVER-2001 creado en Jira";
    private Estatus estatus = new Estatus("0001", "Pendiente por documentacion");
    private string nombreIntermediario = "Wilmar Martinez";
    private string solicitudkey = "JRASERVER-2001";

    public NotificacionSmsRequestDtoBuilder WithNumero(string numero)
    {
        this.numero = numero;
        return this;
    }

    public NotificacionSmsRequestDtoBuilder WithComentario(string comentario)
    {
        this.comentario = comentario;
        return this;
    }

    public NotificacionSmsRequestDtoBuilder WithEstatus(Estatus estatus)
    {
        this.estatus = estatus;
        return this;
    }

    public NotificacionSmsRequestDtoBuilder WithNombreIntermediario(string nombreIntermediario)
    {
        this.nombreIntermediario = nombreIntermediario;
        return this;
    }

    public NotificacionSmsRequestDtoBuilder WithSolicitudkey(string solicitudkey)
    {
        this.solicitudkey = solicitudkey;
        return this;
    }

    public object Build()
    {
        return new
        {
            numero = this.numero,
            comentario = this.comentario,
            estatus = new
            {
                nombre = this.estatus.Nombre,
                id = this.estatus.Id
            },
            nombreIntermediario = this.nombreIntermediario,
            solicitudkey = this.solicitudkey
        };
    }
}
