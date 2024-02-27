using FluentValidation;

namespace GestionCasos.Application.Features.Solicitudes.Commands.CrearSolicitud;
public class CrearSolicitudCommandValidator : AbstractValidator<CrearSolicitudesCommand>
{
    public CrearSolicitudCommandValidator()
    {
        RuleFor(x => x.PrefijoSolicitud)
            .NotEmpty().WithMessage("El prefijo de la solicitud no puede estar vacío.")
            .Matches(@"^[A-Z]{3}-[A-Z]{2}$")
            .WithMessage("El prefijo de la solicitud no tiene la estructura adecuada.");

        RuleFor(x => x.Solicitudes)
            .NotNull()
            .WithMessage("La lista de solicitudes no puede ser nula.")
            .Must(solicitudes => solicitudes.Count > 0)
            .WithMessage("Debe haber al menos una solicitud.")
            .ForEach(solicitudValidator =>
            {
                solicitudValidator.SetValidator(new NuevaSolicitudValidator());
            });
    }
}
public class NuevaSolicitudValidator : AbstractValidator<NuevaSolicitud>
{
    private readonly int CARACTERES_MAX = 500;
    public NuevaSolicitudValidator()
    {
        RuleFor(x => x.Resumen)
            .NotEmpty().WithMessage("El resumen de la solicitud no puede estar vacío.");

        RuleFor(x => x.TipoSolicitud)
            .NotNull().WithMessage("El tipo de solicitud no puede ser nulo.")
            .SetValidator(new TipoSolicitudValidator());

        RuleFor(x => x.ProjectKey)
            .NotEmpty().WithMessage("La clave del proyecto no puede estar vacía.");

        RuleFor(x => x.UsuarioId)
            .NotEmpty().WithMessage("El ID del usuario no puede estar vacío.");

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

        When(command => !string.IsNullOrWhiteSpace(command.Descripcion), () =>
        {
            RuleFor(command => command.Descripcion)
                .MaximumLength(CARACTERES_MAX)
                .WithMessage($"La descripción no puede exceder los {CARACTERES_MAX} caracteres.");
        });
    }
}

public class TipoSolicitudValidator : AbstractValidator<TipoSolicitud>
{
    public TipoSolicitudValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El ID del tipo de solicitud no puede estar vacío.");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del tipo de solicitud no puede estar vacío.");
    }
}
