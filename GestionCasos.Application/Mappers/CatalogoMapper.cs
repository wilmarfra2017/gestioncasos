using GestionCasos.Domain.Dtos;
using GestionCasos.Domain.Entities;
using GestionCasos.Application.Dtos;

namespace GestionCasos.Application.Mappers;
public static class CatalogoMapper
{
    public static ResponsePaginadoDto<CatalogoDto> ToResponsePaginadoDto(PaginadoDto<Catalogo> paginadoCatalogos, int pagina, int tamanoPagina)
    {
        ArgumentNullException.ThrowIfNull(paginadoCatalogos);
        var catalogosDto = paginadoCatalogos.Data.Select(c => c.ToCatalogoResponseDto()).ToList();
        return new ResponsePaginadoDto<CatalogoDto>
        {
            Registros = catalogosDto,
            Pagina = pagina,
            TamanoPagina = tamanoPagina,
            TotalPaginas = paginadoCatalogos.TotalPaginas,
            TotalRegistros = paginadoCatalogos.TotalRegistros
        };
    }
    public static CatalogoDto ToCatalogoResponseDto(this Catalogo catalogoEntity)
    {
        ArgumentNullException.ThrowIfNull(catalogoEntity);
        return new CatalogoDto(catalogoEntity.Nombre, catalogoEntity.Descripcion, catalogoEntity.ToElementoDtos());
    }
    private static List<ElementoDto> ToElementoDtos(this Catalogo catalogo)
    {
        return catalogo.Elementos.Select(elemento => elemento.ToElementoDto()).ToList();
    }

    private static ElementoDto ToElementoDto(this Elemento elemento)
    {
        return new ElementoDto(elemento.Clave, elemento.Valor);
    }
}


