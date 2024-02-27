namespace GestionCasos.Infrastructure.Dtos;

public class ResponseSolicitudesDto
{
    public IList<ReporteSolicitudDto> Issues { get; set; } = default!;
}
