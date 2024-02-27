using FluentValidation;

namespace GestionCasos.Application.Features.Solicitudes.Queries.ObtenerSolicitudes;

public class ObtenerSolicitudesQueryValidator : AbstractValidator<ObtenerSolicitudesQuery>
{
    public ObtenerSolicitudesQueryValidator()
    {
        RuleFor(x => x.UsuarioId)
            .NotEmpty().WithMessage("Id del usuario es requerido.");
    }
}



