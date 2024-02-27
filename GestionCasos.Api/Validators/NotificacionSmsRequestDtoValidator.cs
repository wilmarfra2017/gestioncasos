using FluentValidation;
using GestionCasos.Domain.Dtos;

namespace gestion_casos.Api.Validators;
public class NotificacionSmsRequestDtoValidator : AbstractValidator<NotificacionSmsRequestDto>
{
    public NotificacionSmsRequestDtoValidator()
    {
        RuleFor(n => n.Numero)
            .NotEmpty().WithMessage("El campo numero es requerido y no puede estar vacío.");

        RuleFor(n => n.Comentario)
            .NotEmpty().WithMessage("El campo 'comentario' es requerido y no puede estar vacío.");

        RuleFor(n => n.Estatus.Nombre)
            .NotEmpty().WithMessage("El campo 'estatus.nombre' es requerido y no puede estar vacío.");

        RuleFor(n => n.NombreIntermediario)
            .NotEmpty().WithMessage("El campo 'nombreIntermediario' es requerido y no puede estar vacío.");

        RuleFor(n => n.SolicitudKey)
            .NotEmpty().WithMessage("El campo 'solicitudKey' es requerido y no puede estar vacío.");
    }

}
