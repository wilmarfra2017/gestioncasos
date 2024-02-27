namespace GestionCasos.Domain.Entities;

public class TipoSolicitud
{
    public TipoSolicitud(string id, string nombre)
    {
        Id = id;
        Nombre = nombre;
    }
    public string Nombre { get; init; }
    public string Id { get; init; }
}
