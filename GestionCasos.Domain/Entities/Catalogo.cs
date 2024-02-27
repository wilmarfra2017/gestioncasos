namespace GestionCasos.Domain.Entities;

public class Catalogo : DomainEntity
{
    public string Nombre { get; set; } = default!;
    public string Descripcion { get; set; } = default!;
    public IList<Elemento> Elementos { get; set; } = default!;
}

public class Elemento
{
    public string Clave { get; set; } = default!;
    public string Valor { get; set; } = default!;
}
