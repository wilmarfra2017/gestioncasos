using GestionCasos.Application.Dtos;

namespace GestionCasos.Api.Tests.Catalogos.DataBuilder;

public class CatalogoRequestDtoBuilder
{
    private string nombre = "Nombre predeterminado";
    private string descripcion = "Descripción predeterminada";
    private List<ElementoDto> elementos = new List<ElementoDto>();

    public CatalogoRequestDtoBuilder WithNombre(string nombre)
    {
        this.nombre = nombre;
        return this;
    }

    public CatalogoRequestDtoBuilder WithDescripcion(string descripcion)
    {
        this.descripcion = descripcion;
        return this;
    }

    public CatalogoRequestDtoBuilder AddElemento(ElementoDto elemento)
    {
        this.elementos.Add(elemento);
        return this;
    }

    public CatalogoDto Build()
    {
        return new CatalogoDto(nombre, descripcion, elementos);
    }
}
