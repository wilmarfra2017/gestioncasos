using GestionCasos.Api.Dtos;

namespace GestionCasos.Api.Tests.Solicitudes.DataBuilder;

public class SolicitudesRequestDtoBuilder
{
    private string _prefijoSolicitud = string.Empty;
    private List<SolicitudRequestDto> _solicitudes = new();

    public SolicitudesRequestDtoBuilder ConPrefijoSolicitud(string prefijoSolicitud)
    {
        _prefijoSolicitud = prefijoSolicitud;
        return this;
    }

    public SolicitudesRequestDtoBuilder ConSolicitudes(List<SolicitudRequestDto> solicitudes)
    {
        _solicitudes = solicitudes ?? new List<SolicitudRequestDto>();
        return this;
    }

    public SolicitudesRequestDto Build()
    {
        return new SolicitudesRequestDto(_prefijoSolicitud, _solicitudes);
    }
}
