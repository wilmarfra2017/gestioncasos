using GestionCasos.Domain.Entities;

namespace GestionCasos.Api.Dtos;
public record NotificacionEmailRequestDto(
    string Email,
    string Comentario,
    Estatus Estatus,
    string NombreIntermediario,
    string SolicitudKey
);
