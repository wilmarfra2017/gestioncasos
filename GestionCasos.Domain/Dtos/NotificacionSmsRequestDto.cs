using GestionCasos.Domain.Entities;

namespace GestionCasos.Domain.Dtos;

public record NotificacionSmsRequestDto(string Numero, string Comentario, Estatus Estatus, string NombreIntermediario, string SolicitudKey);

