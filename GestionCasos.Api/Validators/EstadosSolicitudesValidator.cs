using FluentValidation;
using GestionCasos.Application.Features.EstadosSolicitudes.Commands;
using GestionCasos.Domain.Ports;

namespace GestionCasos.Api.Validators;

public class EstadosSolicitudesValidator : AbstractValidator<ActualizarEstadosSolicitudCommand>
{
    public EstadosSolicitudesValidator(ILogMessageService _resourceManager)
    {
        if (_resourceManager == null)
        {
            throw new ArgumentNullException(nameof(_resourceManager), "Resource manager cannot be null.");
        }

        RuleFor(x => x.Key)
            .NotNull().WithMessage(_resourceManager.ErrorValidacionSolicitudKey)
            .NotEmpty().WithMessage(_resourceManager.ErrorValidacionSolicitudKey);

        RuleFor(x => x.Estatus)
            .NotNull().WithMessage(_resourceManager.ErrorValidacionSolicitudEstatus)
            .NotEmpty().WithMessage(_resourceManager.ErrorValidacionSolicitudEstatus);

        RuleFor(x => x.FechaActualizacion)
            .NotNull().WithMessage(_resourceManager.ErrorValidacionFechaActualizacionSolicitud);
    }
}
