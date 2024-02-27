using GestionCasos.Application.Dtos;

namespace GestionCasos.Api.Tests.Catalogos.DataBuilder;

public class ElementoDtoBuilder
{
    private string clave = "Clave predeterminada";
    private string valor = "Valor predeterminado";

    public ElementoDtoBuilder WithClave(string clave)
    {
        this.clave = clave;
        return this;
    }

    public ElementoDtoBuilder WithValor(string valor)
    {
        this.valor = valor;
        return this;
    }

    public ElementoDto Build()
    {
        return new ElementoDto(clave, valor);
    }
}
