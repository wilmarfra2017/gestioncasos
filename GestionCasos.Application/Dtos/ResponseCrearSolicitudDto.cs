namespace GestionCasos.Application.Dtos;

public record ResponseCrearSolicitudDto(
    string SolicitudIdExterna,
    List<SolicitudDto> solicitudesCreadas
);
