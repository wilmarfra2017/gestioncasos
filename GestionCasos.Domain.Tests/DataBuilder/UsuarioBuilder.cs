using GestionCasos.Domain.Entities;

namespace GestionCasos.Domain.Tests.DataBuilder;

public class UsuarioBuilder
{
    private string _id = "defaultId";
    private string _nombre = "defaultNombre";

    public UsuarioBuilder WithId(string id)
    {
        _id = id;
        return this;
    }

    public UsuarioBuilder WithNombre(string nombre)
    {
        _nombre = nombre;
        return this;
    }

    public Usuario Build()
    {
        return new Usuario(_id, _nombre);
    }
}


