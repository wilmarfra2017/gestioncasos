using FluentValidation;

namespace GestionCasos.Application.Features.Notificaciones.Commands.EnviarCorreoNotificacion;

public class EnviarCorreoNotificacionCommandValidator : AbstractValidator<EnviarCorreoNotificacionCommand>
{
    public EnviarCorreoNotificacionCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico no puede estar vacío.")
            .EmailAddress().WithMessage("El correo electrónico no tiene un formato válido.");

        RuleFor(x => x.Comentario)
            .NotEmpty().WithMessage("El mensaje de la notificación no puede estar vacío.");

        RuleFor(x => x.Estado)
            .NotEmpty().WithMessage("El estado de la notificación no puede estar vacío.");
    }
}
