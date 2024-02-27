namespace GestionCasos.Domain.Entities;

public class Usuario
{
    public Usuario(string key, string nombre)
    {
        Id = key;
        Nombre = nombre;
    }
    public string Nombre { get; init; }
    public string Id { get; init; }
}
