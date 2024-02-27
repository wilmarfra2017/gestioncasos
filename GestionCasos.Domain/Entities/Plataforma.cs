namespace GestionCasos.Domain.Entities;

public class Plataforma : DomainEntity
{
    public string Nombre { get; set; } = default!;
    public string Descripcion { get; set; } = default!;
    public string Key { get; set; } = default!;
    public bool Activo { get; set; } = default!;
}
