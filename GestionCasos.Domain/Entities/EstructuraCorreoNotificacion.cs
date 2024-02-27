namespace GestionCasos.Domain.Entities;

public class EstructuraCorreoNotificacion
{
    public string CorreoDestinatario { get; set; } = default!;
    public string NombreProveedor { get; set; } = default!;
    public Plantilla Plantilla { get; set; } = default!;
}
