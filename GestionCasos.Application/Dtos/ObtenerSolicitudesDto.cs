namespace GestionCasos.Application.Dtos;

public record ObtenerSolicitudesDto
(
    string ProyectoKey,
    string SolicitudPadreId,
    string SolicitudId,
    string SolicitudKey,
    string Resumen,
    DateTime FechaCreacion,
    DateTime FechaModificacion,
    TipoSolicitudDto TipoSolicitud,
    EstatusDto Estatus,
    UsuarioDto? UsuarioCreacion,
    string? Descripcion
);
