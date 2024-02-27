using GestionCasos.Domain.Entities;

namespace GestionCasos.Api.Tests.DataBuilder;

public class TipoSolicitudDataBuilder
{
    private string _id = "default_id";
    private string _nombre = "default_nombre";

    public TipoSolicitudDataBuilder WithId(string id)
    {
        _id = id;
        return this;
    }

    public TipoSolicitudDataBuilder WithNombre(string nombre)
    {
        _nombre = nombre;
        return this;
    }

    public TipoSolicitud Build()
    {
        return new TipoSolicitud(_id, _nombre);
    }
}

