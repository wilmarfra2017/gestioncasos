using GestionCasos.Domain.Entities;

namespace GestionCasos.Api.Tests.Notificaciones.DataBuilder;

public class NotificacionEmailRequestDtoBuilder
{
    private string email = "wilframar@gmail.com";
    private string comentario = "Te falta la cedula en el ticket JRASERVER-2000 creado en Jira";
    private Estatus estatus = new Estatus("0001", "Pendiente por documentacion");
    private string nombreIntermediario = "Wilmar Martinez";
    private string solicitudkey = "JRASERVER-2000";

    public NotificacionEmailRequestDtoBuilder WithEmail(string email)
    {
        this.email = email;
        return this;
    }

    public NotificacionEmailRequestDtoBuilder WithComentario(string comentario)
    {
        this.comentario = comentario;
        return this;
    }

    public NotificacionEmailRequestDtoBuilder WithEstatus(Estatus estatus)
    {
        this.estatus = estatus;
        return this;
    }

    public NotificacionEmailRequestDtoBuilder WithNombreIntermediario(string nombreIntermediario)
    {
        this.nombreIntermediario = nombreIntermediario;
        return this;
    }

    public NotificacionEmailRequestDtoBuilder WithSolicitudkey(string solicitudkey)
    {
        this.solicitudkey = solicitudkey;
        return this;
    }

    public object Build()
    {
        return new
        {
            email = this.email,
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
