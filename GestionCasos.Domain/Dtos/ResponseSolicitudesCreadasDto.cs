namespace GestionCasos.Domain.Dtos;

public class ResponseSolicitudesCreadasDto
{
    public string SolicitudIdExterna { get; set; } = default!;
    public IList<ResponseNuevaSolicitudDto> Issues { get; set; } = default!;
}
