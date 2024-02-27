namespace GestionCasos.Api.Dtos;

public record SolicitudesRequestDto(
        string PrefijoSolicitud,
        List<SolicitudRequestDto> Solicitudes
    );


public record SolicitudRequestDto(
        string Resumen,
        TipoSolicitudDto TipoSolicitud,
        string UsuarioId,
        Dictionary<string, object> Campos,
        string? Descripcion
    );

public record TipoSolicitudDto(string Id, string Nombre);
public record CampoDto(string Id, string Valor);
