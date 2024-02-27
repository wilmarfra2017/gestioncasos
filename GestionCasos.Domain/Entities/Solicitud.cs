namespace GestionCasos.Domain.Entities;

public class Solicitud : DomainEntity
{
    public string ProyectoId { get; set; } = default!;
    public string ProyectoKey { get; set; } = default!;
    public string SolicitudPadreId { get; set; } = default!;
    public string SolicitudId { get; set; } = default!;
    public string SolicitudKey { get; set; } = default!;
    public string? Titulo { get; set; }
    public string? Descripcion { get; set; }
    public string Resumen { get; set; } = default!;
    public TipoSolicitud TipoSolicitud { get; set; } = default!;
    public Estatus Estatus { get; set; } = default!;
    public Usuario? UsuarioAsignado { get; set; }
    public Usuario? UsuarioCreacion { get; set; }
    public Usuario? UsuarioModificacion { get; set; }
    public DateTime FechaCreacion { get; set; } = default!;
    public DateTime FechaModificacion { get; set; } = default!;
    public Adjunto? Adjunto { get; set; }
    public string? Comentario { get; set; }

    private static bool ValidarResumen(string resumen) =>
       !string.IsNullOrWhiteSpace(resumen);


    private static bool ValidarSolicitudId(string solicitudId) =>
        !string.IsNullOrWhiteSpace(solicitudId);
    private static bool ValidarSolicitudKey(string solicitudKey) =>
        !string.IsNullOrWhiteSpace(solicitudKey);

    private static bool ValidarSolicitudPadreId(string solicitudPadreId) =>
        !string.IsNullOrWhiteSpace(solicitudPadreId);

    public bool ValidarSolicitud()
    {
        return ValidarResumen(Resumen) &&
            ValidarSolicitudKey(SolicitudKey) &&
            ValidarSolicitudId(SolicitudId) &&
            ValidarSolicitudPadreId(SolicitudPadreId);
    }
}
