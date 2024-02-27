using GestionCasos.Domain.Entities;
using MediatR;

namespace GestionCasos.Application.Features.EstadosSolicitudes.Commands;

public record ActualizarEstadosSolicitudCommand(string Key, string FechaActualizacion, Estatus Estatus, string[]? Comentarios) : IRequest<bool>;
