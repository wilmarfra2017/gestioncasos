using GestionCasos.Application.Dtos;
using MediatR;

namespace GestionCasos.Application.Features.Adjuntos.Commands.AdjuntarDocumento;

public record CargarAdjuntoCommand(
    Stream StreamData,
    string Nombre,
    string Formato,
    long PesoArchivo,
    string SolicitudId,
    string CampoAdjuntoId,
    string? Descripcion
) : IRequest<AdjuntoDto>;
