namespace GestionCasos.Application.Dtos;

public record CatalogoDto(
     string Nombre,
     string Descripcion,
     List<ElementoDto> Elementos
);

public record ElementoDto(

     string Clave,
     string Valor
);
