using GestionCasos.Api.Dtos;

namespace GestionCasos.Api.Tests.Solicitudes.DataBuilder;

public class SolicitudRequestDtoBuilder
{
    private string _resumen = string.Empty;
    private TipoSolicitudDto _tipoSolicitud = new TipoSolicitudDto("", "");
    private string _usuarioId = string.Empty;
    private Dictionary<string, object> _campos = new Dictionary<string, object>();
    private string? _descripcion = null;

    public SolicitudRequestDtoBuilder ConResumen(string resumen)
    {
        _resumen = resumen;
        return this;
    }
    

    public SolicitudRequestDtoBuilder ConTipoSolicitud(TipoSolicitudDto tipoSolicitud)
    {
        _tipoSolicitud = tipoSolicitud;
        return this;
    }

    public SolicitudRequestDtoBuilder ConUsuarioId(string usuarioId)
    {
        _usuarioId = usuarioId;
        return this;
    }

    public SolicitudRequestDtoBuilder ConCampos(Dictionary<string, object> campos)
    {
        _campos = campos;
        return this;
    }

    public SolicitudRequestDtoBuilder ConDescripcion(string? descripcion)
    {
        _descripcion = descripcion;
        return this;
    }

    public SolicitudRequestDto Build()
    {
       return new SolicitudRequestDto(_resumen, _tipoSolicitud, _usuarioId, _campos, _descripcion);
    }
}
