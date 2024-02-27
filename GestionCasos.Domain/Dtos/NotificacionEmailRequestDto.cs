using GestionCasos.Domain.Entities;

namespace GestionCasos.Domain.Dtos;

public record NotificacionEmailRequestDto(string Email, string Comentario, Estatus Estatus, string NombreIntermediario, string SolicitudKey);
