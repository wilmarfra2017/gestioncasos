using FluentValidation;

namespace GestionCasos.Application.Features.Catalogos.Queries.ObtenerCatalogos;

public class ObtenerCatalogosQueryValidator : AbstractValidator<ObtenerCatalogosQuery>
{
    public ObtenerCatalogosQueryValidator()
    {
        RuleFor(x => x)
           .Must(x => new[] { x.Nombre, x.ContieneClave, x.ContieneValor }.ToList().Exists(prop => prop != null))
           .WithMessage($"No existe la consulta");

        RuleFor(x => x.Pagina)
           .GreaterThan(0)
           .WithMessage("El número de página debe ser mayor que cero.");

        RuleFor(x => x.TamanoPagina)
            .GreaterThan(0)
            .WithMessage("El tamaño de página debe ser mayor que cero.");
    }
}
