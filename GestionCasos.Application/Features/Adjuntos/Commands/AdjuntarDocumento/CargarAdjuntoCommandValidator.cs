using FluentValidation;
using GestionCasos.Domain.Ports;

namespace GestionCasos.Application.Features.Adjuntos.Commands.AdjuntarDocumento;

public class CargarAdjuntoCommandValidator : AbstractValidator<CargarAdjuntoCommand>
{
    private readonly int CARACTERES_MAX = 500;
    public CargarAdjuntoCommandValidator(IArchivoConfigService archivoConfigService)
    {
        ArgumentNullException.ThrowIfNull(archivoConfigService);

        int PESO_MAX = archivoConfigService.ObtenerPesoMaximo();
        long PESO_MAX_PERMITIDO = PESO_MAX * 1024 * 1024;

        RuleFor(command => command.StreamData)
            .NotNull()
            .WithMessage("Debe seleccionar un archivo");

        RuleFor(command => command.Nombre)
            .NotEmpty()
            .WithMessage("El nombre del archivo no puede estar vacío.");

        RuleFor(command => command.PesoArchivo)
            .GreaterThan(0)
            .WithMessage("El peso del archivo debe ser mayor a 0.")
            .LessThanOrEqualTo(PESO_MAX_PERMITIDO)
            .WithMessage("El archivo excede el tamaño máximo permitido.");

        RuleFor(command => command.SolicitudId)
            .NotEmpty()
            .WithMessage("El ID de la solicitud es requerido");

        RuleFor(command => command.CampoAdjuntoId)
            .NotEmpty()
            .WithMessage("El ID del campo adjunto es requerido");

        When(command => !string.IsNullOrWhiteSpace(command.Descripcion), () =>
        {
            RuleFor(command => command.Descripcion)
                .MaximumLength(CARACTERES_MAX)
                .WithMessage($"La descripción no puede exceder los {CARACTERES_MAX} caracteres.");
        });
    }
}

