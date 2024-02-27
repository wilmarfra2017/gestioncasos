using FluentValidation;

namespace GestionCasos.Application.Features.Solicitudes.Commands.ActualizarSolicitud;
public class ActualizarSolicitudCommandValidator : AbstractValidator<ActualizarSolicitudCommand>
{
    public ActualizarSolicitudCommandValidator()
    {
        RuleFor(x => x.SolicitudId)
            .NotEmpty().WithMessage("El id de la solicitud no puede estar vacío.");
        
        RuleFor(x => x.Campos)
            .NotNull().WithMessage("Los campos no pueden ser nulos.")
            .Must(campos => campos.Count > 0).WithMessage("Debe haber al menos un campo.")
            .ForEach(dictEntry =>
            {
                dictEntry.ChildRules(pair =>
                {
                    pair.RuleFor(x => x.Key)
                        .NotEmpty().WithMessage("La clave del campo personalizado no puede estar vacía.");
                    pair.RuleFor(x => x.Value)
                        .NotNull().WithMessage("El valor del campo personalizado no puede ser nulo.");
                });
            });
    }
}
