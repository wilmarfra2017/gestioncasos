using FluentValidation;
using GestionCasos.Domain.Dtos;

namespace GestionCasos.Domain.Validators;

public class NotificacionEmailRequestDtoValidator : AbstractValidator<NotificacionEmailRequestDto>
{
    public NotificacionEmailRequestDtoValidator()
    {
        RuleFor(n => n.Email)
            .NotEmpty().WithMessage("El campo 'email' es requerido y no puede estar vacío.")
            .EmailAddress().WithMessage("El campo 'email' debe ser una dirección de correo electrónico válida.");

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

