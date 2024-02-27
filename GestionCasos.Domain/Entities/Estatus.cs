namespace GestionCasos.Domain.Entities;

public class Estatus
{
    public Estatus(string id, string nombre)
    {
        Id = id;
        Nombre = nombre;
    }
    public string Nombre { get; init; }
    public string Id { get; init; }
}
