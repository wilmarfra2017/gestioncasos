using FluentValidation;
using GestionCasos.Application.Features.Notificaciones.Commands.EnviarSmsNotificacion;

namespace gestion_casos.Application.Features.Notificaciones.Commands.EnviarSmsNotificacion;

public class EnviarSmsNotificacionCommandValidator : AbstractValidator<EnviarSmsNotificacionCommand>
{
    public EnviarSmsNotificacionCommandValidator()
    {
        RuleFor(x => x.Numero)
           .NotEmpty().WithMessage("El numero de celular no puede estar vacío.");           

        RuleFor(x => x.Comentario)
            .NotEmpty().WithMessage("El mensaje de la notificación no puede estar vacío.");

        RuleFor(x => x.Estado)
            .NotEmpty().WithMessage("El estado de la notificación no puede estar vacío.");
    }
}
