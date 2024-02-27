namespace GestionCasos.Domain.Entities;

public class EstructuraSmsNotificacion
{
    public string NumeroDestinatario { get; set; } = default!;
    public string NombreProveedor { get; set; } = default!;
    public Plantilla Plantilla { get; set; } = default!;
}
